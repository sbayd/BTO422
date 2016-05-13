using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using com.refractored;
using Java.Interop;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using String = Java.Lang.String;
using Android.Speech.Tts;

namespace VictoriaGame
{
    [Activity(Label = "GameSelection", MainLauncher = true, Icon = "@drawable/icon")]
    public class GameSelectionActivity : BaseActivity, IOnTabReselectedListener, ViewPager.IOnPageChangeListener, TextToSpeech.IOnInitListener
    {
        private GameSelectionPagerAdapter adapter;
        private int currentColor;
        private Drawable oldBackground;
        private ViewPager pager;
        private PagerSlidingTabStrip tabs;
        TextToSpeech tts;

        protected override int LayoutResource
        {
            get { return Resource.Layout.activity_game_selection; }
        }


        public void OnPageScrollStateChanged(int state)
        {
            Console.WriteLine("Page scroll state changed: " + state);
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            Console.WriteLine("Page Scrolled");
        }

        public void OnPageSelected(int position)
        {
            Console.WriteLine("Page selected: " + position);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            adapter = new GameSelectionPagerAdapter(SupportFragmentManager);
            pager = FindViewById<ViewPager>(Resource.Id.game_selection_pager);
            tabs = FindViewById<PagerSlidingTabStrip>(Resource.Id.game_Selection_tabs);
            pager.Adapter = adapter;
            tabs.SetViewPager(pager);
            var pageMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Resources.DisplayMetrics);
            pager.PageMargin = pageMargin;
            pager.CurrentItem = 0;
            tabs.OnTabReselectedListener = this;
            tabs.OnPageChangeListener = this;

            tts = new TextToSpeech(this, this);
            ChangeColor(Resources.GetColor(Resource.Color.green));

        }
        void TextToSpeech.IOnInitListener.OnInit(OperationResult status)
        {
            // if we get an error, default to the default language
            if (status == OperationResult.Error)
                tts.SetLanguage(Java.Util.Locale.Default);
            // if the listener is ok, set the lang
            if (status == OperationResult.Success)
                tts.SetLanguage(new Java.Util.Locale("tr-TR"));
            speak("Hoş geldin! Oyun boyunca sana yardımcı olmaya çalışacağım.", QueueMode.Add);
            speak("Ekrana bir kere tıklayarak seviye hakkında bilgi alabilirsin.", QueueMode.Add);

        }
        private void ChangeColor(Color newColor)
        {
            tabs.SetBackgroundColor(newColor);

            // change ActionBar color just if an ActionBar is available
            Drawable colorDrawable = new ColorDrawable(newColor);
            Drawable bottomDrawable = new ColorDrawable(Resources.GetColor(Android.Resource.Color.Transparent));
            var ld = new LayerDrawable(new[] { colorDrawable, bottomDrawable });
            if (oldBackground == null)
            {
            }
            else
            {
                var td = new TransitionDrawable(new[] { oldBackground, ld });
                td.StartTransition(200);
            }

            oldBackground = ld;
            currentColor = newColor;
        }

        [Export("onColorClicked")]
        public void OnColorClicked(View v)
        {
            var color = Color.ParseColor(v.Tag.ToString());
            ChangeColor(color);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("currentColor", currentColor);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            currentColor = savedInstanceState.GetInt("currentColor");
            ChangeColor(new Color(currentColor));
        }

        #region IOnTabReselectedListener implementation

        public void OnTabReselected(int position)
        {
            Toast.MakeText(this, "Tab reselected: " + position, ToastLength.Short).Show();
        }

        public void speak(string text, QueueMode qm = QueueMode.Flush)
        {
         
            tts.Speak(text, qm, null);
        }
        #endregion


    }

    public class GameSelectionPagerAdapter : FragmentPagerAdapter
    {
        private readonly string[] Titles =
        {
            //TO DO: init from db.
            "Victoria", "Nightmare", "Stranger"
        };



        public GameSelectionPagerAdapter(FragmentManager fm)
            : base(fm)
        {
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String(Titles[position]);
        }

        #region implemented abstract members of PagerAdapter

        public override int Count
        {
            get { return Titles.Length; }
        }

        #endregion

        #region implemented abstract members of FragmentPagerAdapter

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 2:
                    return GameSelectionCardFragment.NewInstance(position);
                    break;
                default:
                    return GameSelectionCardFragment.NewInstance(position);
                    break;

            }
        }

        #endregion


    }
}