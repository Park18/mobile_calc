using System;
using System.Collections.Generic;
using System.Text;

namespace mobile_calc
{
    /// <summary>
    /// 계산기의 메인 클래스
    /// </summary>
    class Calc
    {
        /// <summary>
        /// 매개변수로 피연산자 x와 y, 연산자 tmp_operator를 받고
        /// x 연산자 y를 계산하여 반환한다.
        /// </summary>
        /// <param name="x">피연산자 x</param>
        /// <param name="y">피연산자 y</param>
        /// <param name="tmp_operator">연산자</param>
        /// <returns>연산 결과</returns>
        public double Operate(double x, double y, char tmp_operator)
        {
            if (tmp_operator == '+')
            {
                return x + y;
            }

            else if (tmp_operator == '-')
            {
                return x - y;
            }

            else if (tmp_operator == '*')
            {
                return x * y;
            }

            else
            {
                return x / y;
            }
        }

        /// <summary>
        /// 매개변수로 받은 값이 옳바른 값인지 확인한다.
        /// 옳바른 값이면 true, 틀린 값이면 false를 반환한다.
        /// </summary>
        /// <param name="num">double형 숫자</param>
        /// <returns></returns>
        public bool Is_Right(double num)
        {

            if (double.IsInfinity(num) || double.IsNaN(num))
            {
                return false;
            }

            else
            {
                return true;
            }
        }
    }
}
