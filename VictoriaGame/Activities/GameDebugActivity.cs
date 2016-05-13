using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OpenCV.Android;
using OpenCV.ImgProc;
using OpenCV.Core;
using System.Threading.Tasks;
using Android.Speech.Tts;
using Android.Media;
using Android.Graphics;
using ZXing;
using Java.IO;
using ZXing.Common;
using ZXing.Datamatrix;
using Java.Nio;
using ZXing.Mobile;

namespace VictoriaGame.Activities
{
    [Activity(Label = "GameDebugActivity")]
    public class GameDebugActivity : Activity, TextToSpeech.IOnInitListener, MediaPlayer.IOnCompletionListener
    {
        private Mat mIntermediateMat;
        private TextView _wordText;

        string currentText = string.Empty;
        int currentChar = 0;
        MobileBarcodeScanner scanner;

        TextToSpeech tts;
        MediaPlayer mp;
        Dictionary<int, char> letters = new Dictionary<int, char>();
        Dictionary<int, int> notes = new Dictionary<int, int>();
        List<char> founded = new List<char>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);

            Window.SetFlags(Android.Views.WindowManagerFlags.Fullscreen, Android.Views.WindowManagerFlags.Fullscreen);

            SetContentView(Resource.Layout.game_screen);

            _wordText = FindViewById<TextView>(Resource.Id.game_screen_text_view);
            View decorView = Window.DecorView;
            var uiOptions = (int)decorView.SystemUiVisibility;
            var newUiOptions = (int)uiOptions;

            newUiOptions |= (int)SystemUiFlags.LowProfile;
            newUiOptions |= (int)SystemUiFlags.Fullscreen;
            newUiOptions |= (int)SystemUiFlags.HideNavigation;
            newUiOptions |= (int)SystemUiFlags.Immersive;
            tts = new TextToSpeech(this, this);
            decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;

            initLetters();


            scanner.UseCustomOverlay = false;


            var opt = new MobileBarcodeScanningOptions();
            opt.DelayBetweenContinuousScans = 3000;

            //Start scanning
            scanner.ScanContinuously(opt, HandleScanResult);

        }
        void TextToSpeech.IOnInitListener.OnInit(OperationResult status)
        {
            // if we get an error, default to the default language
            if (status == OperationResult.Error)
                tts.SetLanguage(Java.Util.Locale.Default);
            // if the listener is ok, set the lang
            if (status == OperationResult.Success)
                tts.SetLanguage(new Java.Util.Locale("tr-TR"));
            speak("Ýlk kelime : ELMA", QueueMode.Flush);
            playSoundForText("APPLE");

        }
        void MediaPlayer.IOnCompletionListener.OnCompletion(MediaPlayer player)
        {
            currentChar++;
            if (currentText.Length > currentChar)
            {
                mp = MediaPlayer.Create(this, notes[currentChar]);
                mp.SetOnCompletionListener(this);
                mp.Start();
            }
        }
        private void initLetters()
        {


            //letters[getEqualofRGB(0, 1, 0)] = 'C';
            //letters[getEqualofRGB(0, 2, 0)] = 'B';
            //letters[getEqualofRGB(1, 1, 0)] = 'A';
            //letters[getEqualofRGB(2, 1, 0)] = 'E';
            //letters[getEqualofRGB(2, 0, 0)] = 'D';
            //letters[getEqualofRGB(0, 3, 0)] = 'P';
            //letters[getEqualofRGB(1, 3, 0)] = 'R';
            //letters[getEqualofRGB(2, 2, 0)] = 'L';
            //letters[getEqualofRGB(1, 2, 0)] = 'N';
            //letters[getEqualofRGB(3, 2, 0)] = 'H';
            //letters[getEqualofRGB(2, 3, 0)] = 'O';
            //letters[getEqualofRGB(3, 3, 0)] = 'G';
            //letters[getEqualofRGB(3, 1, 0)] = 'L';
            //letters[getEqualofRGB(1, 0, 0)] = 'T';
            //letters[getEqualofRGB(3, 0, 0)] = 'K';


            notes.Add(0, Resource.Raw.nota_do);
            notes.Add(1, Resource.Raw.nota_re);
            notes.Add(2, Resource.Raw.nota_mi);
            notes.Add(3, Resource.Raw.nota_fa);
            notes.Add(4, Resource.Raw.nota_sol);
            notes.Add(5, Resource.Raw.nota_la);
            notes.Add(6, Resource.Raw.nota_si);



        }

        protected override void OnPause()
        {
            base.OnPause();

        }

        protected override void OnResume()
        {
            base.OnResume();



        }
        private void playSoundForText(string text)
        {
            int c = text.Length;
            currentText = text;
            mp = MediaPlayer.Create(this, notes[0]);
            mp.SetOnCompletionListener(this);
            mp.Start();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

        }
        public void speak(string text, QueueMode qm = QueueMode.Flush)
        {

            tts.Speak(text, qm, null);
        }



        private char GetLetter(int r = 0, int g = 0, int b = 0)
        {
            try
            {
                return 'A';
            }
            catch (Exception e)
            {
                return '1';
            }
        }

        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
                msg = "Found Barcode: " + result.Text;
            else
                msg = "Scanning Canceled!";

            this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
        }


    }

}