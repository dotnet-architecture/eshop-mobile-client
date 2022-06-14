using eShopOnContainers.Animations.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace eShopOnContainers.Animations
{
    [ContentProperty("Animations")]
    public class StoryBoard : AnimationBase
    {
        public StoryBoard()
        {
            Animations = new List<AnimationBase>();
        }

        public StoryBoard(List<AnimationBase> animations)
        {
            Animations = animations;
        }

        public List<AnimationBase> Animations
        {
            get;
        }

        protected override async Task BeginAnimation()
        {
            foreach (var animation in Animations)
            {
                if (animation.Target == null)
                    animation.Target = Target;

                await animation.Begin();
            }
        }

        protected override async Task ResetAnimation()
        {
            foreach (var animation in Animations)
            {
                if (animation.Target == null)
                    animation.Target = Target;

                await animation.Reset();
            }
        }
    }
}
