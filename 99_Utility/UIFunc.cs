using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Utility
{
    public static class UIFunc
    {
        public static void LerpColor(VisualElement obj, Color target, float speed, float distance)
        {
            if (Vector4.Distance(target, obj.resolvedStyle.backgroundColor) > distance)
            {
                obj.style.backgroundColor = Color.Lerp(obj.resolvedStyle.backgroundColor, target, speed);
            }
        }

        public static bool TypingFX(TextElement obj, char[] chars, ref int index, float spacing, ref float currTime)
        {
            if (chars?.Length > index)
            {
                currTime += Time.deltaTime;

                if (currTime >= spacing)
                {
                    currTime = 0;
                    obj.text += chars[index];

                    index++;
                }

                return true;
            }
            else return false;
        }
    }
}