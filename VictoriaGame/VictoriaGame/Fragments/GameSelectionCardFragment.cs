using Android.Support.V4.App;
using Android.OS;
using Android.Support.V4.View;


using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Content;
using Android.Speech.Tts;

namespace VictoriaGame
{


    public class GameSelectionCardFragment : Fragment
    {
        private int position;
        private ImageView _imageView;
        private int clickCount = 0;
        public static GameSelectionCardFragment NewInstance(int position)
        {
            var f = new GameSelectionCardFragment();
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
            var root = inflater.Inflate(Resource.Layout.game_selection_card, container, false);
            _imageView = root.FindViewById<ImageView>(Resource.Id.game_image_view);
            _imageView.Click += btn_Click;
            ViewCompat.SetElevation(root, 50);

            return root;
        }

        void speakOnMain(string text, QueueMode q = QueueMode.Flush)
        {
            ((GameSelectionActivity)Activity).speak(text, q);
        }
        void btn_Click(object sender, System.EventArgs e)
        {
            if (clickCount == 0)
            {
                speakOnMain("Birinci seviye, zafer. Bu seviyeyi henüz tamamlamadın.", QueueMode.Flush);
                speakOnMain("Kategori seçmek için tekrar dokun.", QueueMode.Add);

                clickCount++;
            }
            else if (clickCount == 1)
            {
             
                Intent newActivity = new Intent(this.Context, typeof(CategorySelectionActivity));

                StartActivity(newActivity);
            }


        }
    }
}

