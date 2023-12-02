using System.Collections;
using UnityEngine;

namespace Assets.Code.LOGIC.Player
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        [Header("components")]
        [SerializeField]
        private AudioController _audioController;


        public void PlayStepSound() => _audioController.PlayWithPitchVariation(AudioController.ClipName.Player_Step);
        public void PlayJumpSound()
        {
            _audioController.PlayClip(AudioController.ClipName.Player_Jump);
        }
        public void PlayFartSound() => _audioController.PlayWithPitchVariation(AudioController.ClipName.Player_Fart);
        public void PlayPoopSound() => _audioController.PlayClip(AudioController.ClipName.Player_Poop);

    }
}