using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMacroSociety.Randoms
{
    public class CreateVerificationCode
    {
        public int RandomInt(int size)
        {
            Random random = new Random();
            int result = 0;
            for (int i = 0; i < size; i++)
            {
                //Генерируем число от 0 до 9, заполняем им разряд.
                result = (int)((result * 10) + (random.NextDouble() * 9));

                //Целое число не может начинаться с 0, если его разрядность больше 1
                if (size > 1 && result == 0)
                    result++;
            }
            return result;
        }
    }
}
