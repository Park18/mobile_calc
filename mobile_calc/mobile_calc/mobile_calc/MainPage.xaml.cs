using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mobile_calc
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private bool is_sign;   // ±를 설정을 저장하는 변수, +: false / -: treu
        private bool is_point;  // 소수점 설정을 저장하는 변수, 존재: treu / 미존재: false
        private char _operator; // 연산자를 임시로 저장하는 변수
        private double tmp_result; // 계산 결과를 임시로 저장하는 변수

        public MainPage()
        {
            InitializeComponent();
            this.Init();
        }

        private void Init()
        {
            this.is_point = false;
            this.is_sign = false;
            this._operator = 'N';
            this.tmp_result = 0;

            polynomial_label.Text = "";
            tmp_label.Text = "0";
        }

        private double Calculate()
        {
            double result = 0;

            if (this._operator == '+')
            {
                result = this.tmp_result + Convert.ToDouble(tmp_label.Text);
            }

            else if (this._operator == '-')
            {
                result = this.tmp_result - Convert.ToDouble(tmp_label.Text);
            }

            else if (this._operator == '*')
            {
                result = this.tmp_result * Convert.ToDouble(tmp_label.Text);
            }

            else
            {
                try
                {
                    result = this.tmp_result / Convert.ToDouble(tmp_label.Text);
                }

                catch
                {
                    this.Init();
                    tmp_label.Text = "0으로 나눌 수 없습니다.";
                }
            }

            this.tmp_result = result;
            return result;
        }

        private bool Is_Int(double result)
        {
            int tmp_result = (int)result;

            if ((double)tmp_result == result)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /**
         * @brief   숫자 버튼 이벤트
         * @details 버튼에 해당하는 숫자가 
         *          tmp_label 입력된다.
         * @param   sender 이벤트 발생 대상
         * @param   e 이벤트 종류
         */
        private void Num_Clicked(object sender, EventArgs e)
        {
            // main_label의 string이 0일 때 공백으로 변경
            if (tmp_label.Text.Equals("0"))
            {
                tmp_label.Text = "";
            }

            // object로 넘어온 sender를 Button으로 형변환
            // 근데 왜 (Button)sender가 아닌 ((Button)sender)인지 잘 모르겠음
            tmp_label.Text = tmp_label.Text + ((Button)sender).Text;
        }

        /**
         * @brief   소수점 버튼 이벤트
         * @details 
         * @param   sender 이벤트 발생 대상
         * @param   e 이벤트 종류
         */
        private void Point_Clicked(object sender, EventArgs e)
        {
            if (tmp_label.Text.Equals("0"))
            {
                return;
            }

            // 소수점이 없을 때
            if (this.is_point == false)
            {
                this.is_point = true;
                tmp_label.Text = tmp_label.Text + ".";
            }
        }

        /**
          * @brief   부호(±) 버튼 이벤트
          * @details 
          * @param   sender 이벤트 발생 대상
          * @param   e 이벤트 종류
          */
        private void Sign_Clicked(object sender, EventArgs e)
        {
            if (tmp_label.Text.Equals("0"))
            {
                return;
            }

            if (this.is_sign == false)
            {
                this.is_sign = true;
                tmp_label.Text = "-" + tmp_label.Text;
            }

            else
            {
                this.is_sign = false;
                tmp_label.Text = tmp_label.Text.Substring(1);
            }
        }

        /**
           * @brief   초기화 이벤트
           * @details 
           * @param   sender 이벤트 발생 대상
           * @param   e 이벤트 종류
           */
        private void Clear_Clicked(object sender, EventArgs e)
        {
            this.Init();
        }

        /**
          * @brief   입력중인 값 초기화 이벤트
          * @details ㅇ
          * @param   sender 이벤트 발생 대상
          * @param   e 이벤트 종류
          */
        private void Clear_Entry_Clicked(object sender, EventArgs e)
        {
            this.is_sign = false;
            this.is_point = false;
            tmp_label.Text = "0";
        }

        /**
          * @brief   사칙연산 버튼 이벤트
          * @details 계산이 연속적으로 된다.
          *          예) 15+45+26+ ... + 입력된 숫자
          * @param   sender 이벤트 발생 대상
          * @param   e 이벤트 종류
          */
        private void Calc_Clicked(object sender, EventArgs e)
        {
            if (polynomial_label.Text.Equals(""))
            {
                this._operator = ((Button)sender).Text[0];
                polynomial_label.Text = tmp_label.Text + ((Button)sender).Text;

                tmp_label.Text = "0";
            }

            else
            {
                this._operator = ((Button)sender).Text[0];
                polynomial_label.Text = polynomial_label.Text + tmp_label.Text + ((Button)sender).Text;
                this.tmp_result = this.Calculate();

                tmp_label.Text = Convert.ToString(this.tmp_result);
            }
        }

        /**
          * @brief   계산(=) 버튼 이벤트
          * @details 계산이 연속적이지 않다.
          *          예) 
          * @param   sender 이벤트 발생 대상
          * @param   e 이벤트 종류
          */
        private void Result_Clicked(object sender, EventArgs e)
        {

        }
    }
}
