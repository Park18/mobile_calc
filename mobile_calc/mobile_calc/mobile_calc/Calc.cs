using System;
using System.Collections.Generic;
using System.Text;

namespace mobile_calc
{
    class Calc
    {
        private double tmp_result;

        public Calc()
        {
            Set_Tmp_Result(0);
        }

        public void Set_Tmp_Result(double result)
        {
            this.tmp_result = result;
        }

        public double Get_Tmp_Result()
        {
            return this.tmp_result;
        }

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

        public bool Is_Right(double num)
        {
            if (double.IsNaN(num))
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        public bool Is_Int(double num)
        {
            int tmp_num = (int)num;

            if((double)tmp_num == num)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
