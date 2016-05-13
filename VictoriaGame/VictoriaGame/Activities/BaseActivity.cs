using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace VictoriaGame
{
    public abstract class BaseActivity : AppCompatActivity
    {

        protected abstract int LayoutResource { get; }



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);

            Window.DecorView.SystemUiVisibility = Android.Views.StatusBarVisibility.Hidden;
            Window.SetFlags(Android.Views.WindowManagerFlags.Fullscreen, Android.Views.WindowManagerFlags.Fullscreen);
            View decorView = Window.DecorView;
            var uiOptions = (int)decorView.SystemUiVisibility;
            var newUiOptions = (int)uiOptions;

            newUiOptions |= (int)SystemUiFlags.LowProfile;
            newUiOptions |= (int)SystemUiFlags.Fullscreen;
            newUiOptions |= (int)SystemUiFlags.HideNavigation;
            newUiOptions |= (int)SystemUiFlags.Immersive;

            decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;

            SetContentView(LayoutResource);
        }
    }
}