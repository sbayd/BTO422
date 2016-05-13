using Android.Support.V4.App;
using Android.OS;
using Android.Support.V4.View;


using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Content;
using VictoriaGame.Activities;
using Android.Speech.Tts;

namespace VictoriaGame
{


    public class CategorySelectionCardFragment : Fragment
    {
        private int position;
        private ImageView _imageView;
        private int clickCount = 0;
        public static CategorySelectionCardFragment NewInstance(int position)
        {
            var f = new CategorySelectionCardFragment();
            var b = new Bundle();
            b.PutInt("position", position);
            f.Arguments = b;
            return f;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            position = Arguments.GetInt("position");
        }


        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.category_selection_card, container, false);
            _imageView = root.FindViewById<ImageView>(Resource.Id.category_image_view);
            _imageView.Click += btn_Click;
            ViewCompat.SetElevation(root, 50);
            return root;
        }

        void speakOnMain(string text, QueueMode q = QueueMode.Flush)
        {
            ((CategorySelectionActivity)Activity).speak(text, q);
        }
        void btn_Click(object sender, System.EventArgs e)
        {
            if (clickCount == 0)
            {
                speakOnMain("Meyveler", QueueMode.Flush);
                speakOnMain("Bu kategoriyi henüz tamamlamadın.", QueueMode.Add);
                speakOnMain("Oyuna başlamak için tekrar dokunun.", QueueMode.Add);

                clickCount++;
            }
            else
            {
                if (position == 2)
                {
                    Intent newActivity = new Intent(this.Context, typeof(GameDebugActivity));

                    StartActivity(newActivity);
                }
                else
                {
                    Intent newActivity = new Intent(this.Context, typeof(GameActivity));

                    StartActivity(newActivity);
                }
            }
        }
    }
}

