using UnityEngine;
using System.Runtime.InteropServices;

namespace Menus.OpenLinks {
    public class OpenLinks : MonoBehaviour {
        [DllImport("__Internal")]
        private static extern void OpenTab(string url);

        private static void OpenUrl(string url) {
            // run only from WebGl builds
            #if !UNITY_EDITOR && UNITY_WEBGL
			OpenTab(url);
            #endif
        }

        public void GoToSebyGit() {
            OpenUrl("https://github.com/sebastianpacurar/Tower-Offender_2D-top-down");
        }

        public void GoToZintoki() {
            OpenUrl("https://zintoki.itch.io/");
        }

        public void GoToFreeGameAssets() {
            OpenUrl("https://free-game-assets.itch.io/");
        }

        public void GoToDariDevTm() {
            OpenUrl("https://daridevtm.itch.io/");
        }

        public void GoToIdznak() {
            OpenUrl("https://idznak.itch.io/");
        }

        public void GoToCaz() {
            OpenUrl("https://cazwolf.itch.io/");
        }

        public void GoToNicolemariet() {
            OpenUrl("https://nicolemariet.itch.io/");
        }

        public void GoToMikiz() {
            OpenUrl("https://mikiz.itch.io/");
        }
    }
}