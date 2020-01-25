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
        private bool is_point;  // 소수점 설정을 저장하는 변수, 존재: treu / 미존재: false
        private bool is_sign;   // ±를 설정을 저장하는 변수, +: false / -: treu
        private bool is_first;  // 첫 번째 사칙연산 버튼을 누른 것인지 설정을 저장하는 변수
                                // 첫 번째 연산: true / 두 번째 이상 연산: false
        private bool is_last;   // 마지막 연산
        private bool is_result; // tmp_label 에 출력된 값이 결과 값인지 설정을 저장하는 변수
                                // 결과 값: ture / 입력 값: false
        private double tmp_result;  // 결과 값을 임시로 저장하는 변수
        private char tmp_operator;  // 연산자를 임시로 저장하는 변수
        private Calc calc = new Calc();

        public MainPage()
        {
            InitializeComponent();
            this.Init();
        }

        /**
         * @brief   클래스 멤버 변수 초기화 메소드
         */
        private void Init()
        {
            this.is_point = false;
            this.is_sign = false;
            this.is_first = true;
            this.is_last = false;
            this.is_result = false;
            this.tmp_result = 0;
            this.tmp_operator = 'N';
        }

        /**
         * @brief       숫자 버튼 이벤트
         * @details     버튼에 해당하는 숫자가 
         *              tmp_label 입력된다.
         * @param   sender 이벤트 발생 대상
         * @param   e 이벤트 종류
         */
        private void Num_Clicked(object sender, EventArgs e)
        {
            // 숫자 버튼이 눌렸을 때 tmp_label의 숫자가 0이면 
            // 0대신 공백으로 설정한다.
            if(tmp_label.Text == "0" || this.is_result)
            {
                this.is_result = false;
                tmp_label.Text = "";

                if(this.is_last == true)
                {
                    this.is_last = false;

                    polynomial_label.Text = "";
                    this.Init();
                }
            }

            else if(tmp_label.Text=="-0")
            {
                tmp_label.Text = "-";
            }

            // 숫자 버튼의 해당하는 수를 tmp_label에 추가한다.
            tmp_label.Text = tmp_label.Text + ((Button)sender).Text;
        }

        /**
         * @brief   소수점 버튼 이벤트
         * @details 소수점 없을 때
         *          - 넣는다.
         *          소주점 있을 때
         *          - 소수점 위치가 마지막 위치이면 소수점 삭제
         *          - 소수점 위치가 마지막 위치가 아니면 반응 x
         * @param   sender 이벤트 발생 대상
         * @param   e 이벤트 종류
         */
        private void Point_Clicked(object sender, EventArgs e)
        {
            // is_point 가 해제되어있으면 설정하고 tmp_label에 소수점(".")을 추가한다.
            if(this.is_point == false)
            {
                this.is_point = true;
                tmp_label.Text = tmp_label.Text + ".";
            }

            // is_point 가 설정되어있으면
            else
            {
                // 소수점의 위치가 tmp_label의 맨 마지막이면 is_point 를 해제하고 소수점(".")을 제거한다.
                if(tmp_label.Text.IndexOf('.') == tmp_label.Text.Length - 1)
                {
                    this.is_point = false;
                    tmp_label.Text = tmp_label.Text.Substring(0, tmp_label.Text.Length - 1);
                }

                // 소수점의 위치가 tmp_label의 맨 마지막이 아니면 아무 행동도 하지 않는다.
                else
                {
                    return;
                }
            }
        }

        /**
          * @brief      부호(±) 버튼 이벤트
          * @details    
          * @param      sender 이벤트 발생 대상
          * @param      e 이벤트 종류
          */
        private void Sign_Clicked(object sender, EventArgs e)
        {
            // is_sign 이 해제되어있으면 설정하고 tmp_label의 맨 앞에 음의 부호("-")를 추가한다.
            if(this.is_sign == false)
            {
                this.is_sign = true;
                tmp_label.Text = "-" + tmp_label.Text;
            }

            // is_sign 이 설정되어있으면 해제하고 tmp_label의 음의 부호를 제거한다.
            else
            {
                this.is_sign = false;
                tmp_label.Text = tmp_label.Text.Substring(1);
            }

            // 현재 tmp_label 에 입력된 값이 결과 값이면 is_result 를 해제하고 is_sign 를 설정한다.
            // tmp_label 의 값을 "-0"으로 변경한다.
            if(this.is_result == true)
            {
                this.is_result = false;
                this.is_sign = true;
                tmp_label.Text = "-0";
            }
        }

        /**
           * @brief     초기화 이벤트
           * @details   모든 값을 초기화 한다.
           * @param     sender 이벤트 발생 대상
           * @param     e 이벤트 종류
           */
        private void Clear_Clicked(object sender, EventArgs e)
        {
            this.Init();
            polynomial_label.Text = "";
            tmp_label.Text = "0";
        }

        /**
          * @brief   입력중인 값 초기화 이벤트
          * @details tmp_label의 값에 입력된 값만 초기화 한다.
          * @param   sender 이벤트 발생 대상
          * @param   e 이벤트 종류
          */
        private void Clear_Entry_Clicked(object sender, EventArgs e)
        {
            this.Init();
            tmp_label.Text = "0";
        }

        /**
          * @brief   사칙연산 버튼 이벤트
          * @details 계산이 연속적으로 된다.
          *          예) 15+45+26+ ... + 입력된 숫자
          * @param   sender 이벤트 발생 대상
          * @param   e 이벤트 종류
          * @detales 
          */
        private void Calc_Clicked(object sender, EventArgs e)
        {
            if(this.is_last == true)
            {
                this.is_last = false;
                this.is_first = false;

                this.tmp_operator = ((Button)sender).Text[0];
                this.tmp_result = Convert.ToDouble(tmp_label.Text);

                polynomial_label.Text = tmp_label.Text + this.tmp_operator;
                tmp_label.Text = "0";
            }


            // 첫 번째 입력이면 
            // 1. 첫 번째 입력이 아님을 알리기 위해 is_first 설정을 해제한다.
            // 2. tmp_operator 에 연산자를 저장한다.
            // 3. tmp_result 에 x(피연산자)를 저장한다.
            // 4. polynomial_label 에 연산식을 작성한다.
            // 5. y(피연산자)의 입력을 위해 tmp_label 에 "0"을 작성한다.
            else if (this.is_first == true)
            {
                this.is_first = false;

                this.tmp_operator = ((Button)sender).Text[0];
                this.tmp_result = Convert.ToDouble(tmp_label.Text);

                polynomial_label.Text = polynomial_label.Text + tmp_label.Text + this.tmp_operator;
                tmp_label.Text = "0";
            }

            // 첫 번째 입력이 아니면
            // 1. tmp_label 에 작성된 값이 결과 값임을 알리기 위해  is_result 를 설정한다.
            // 2. tmp_result에 x(tmp_result) 연산자(tmp_operator(전에 입력한 연산자)) y(현재 입력된 피연산자) 로 결과 값을 저장한다.
            //  예1) 연산식: 0.3 * 2 - 일 때
            //       x = 0.3, 연산자 = *, y = 2 이다.
            // 3. tmp_operator 에 현재 입력된 연산자를 저장한다.
            //  예2) 위의 예1)의 상황일 때 
            //       현재 입력된 연산자는 "-" 이다.
            // 4. polynomial_label 에 연산식을 작성한다.
            // 5. tmp_label에 결과 값을 작성한다.
            else
            {
                this.is_result = true;

                this.tmp_result = calc.Operate(this.tmp_result, Convert.ToDouble(tmp_label.Text), this.tmp_operator);
                this.tmp_operator = ((Button)sender).Text[0];

                polynomial_label.Text = polynomial_label.Text + tmp_label.Text + this.tmp_operator;
                tmp_label.Text = Convert.ToString(this.tmp_result);

            }

            // 다음 피연산자의 입력을 위해 is_sign, is_point 설정을 해제한다.
            this.is_sign = false;
            this.is_point = false;
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
            // 첫 번째 사칙연산 입력 전이면 아무 행동하지 않는다.
            if (this.is_first == true)
            {
                return;
            }

            // 첫 번쨰 사칙 연산 입력 후이면
            // 1. is_result, is_last 를 설정한다.
            // 2. polyomial_label 에 연산식을 작성한다.
            // 3. 결과 값을 연산한다.
            // 4. tmp_label 에 결과 값을 출력한다.
            else
            {
                this.is_result = true;
                this.is_last = true;

                polynomial_label.Text = polynomial_label.Text + tmp_label.Text;
                this.tmp_result = calc.Operate(this.tmp_result, Convert.ToDouble(tmp_label.Text), this.tmp_operator);
                tmp_label.Text = Convert.ToString(this.tmp_result);
            }

        }

        /**
          * @brief      뒤로가기 버튼 이벤트
          * @details    tmp_label의 숫자를 뒤에서부터 하나씩 없앤다.
          *             소수점이나 부호가 있으면 없애면서 초기화를 한다.
          * @param   sender 이벤트 발생 대상
          * @param   e 이벤트 종류
          */
        private void Back_Clicked(object sender, EventArgs e)
        {
            // tmp_label의 숫자 맨 마지막이 '.'이면 is_point 를 해제한다.
            if(tmp_label.Text.LastIndexOf('.') == tmp_label.Text.Length - 1)
            {
                this.is_point = false;
            }

            // tmp_label의 숫자 뒤에서 하나 삭제
            tmp_label.Text = tmp_label.Text.Substring(0, tmp_label.Text.Length - 1);

            // tmp_label의 길이가 0이 되거나 tmp_label이 "-"이면 
            // is_sign 을 해제하고 tmp_label을 "0"으로 설정한다.
            if(tmp_label.Text.Length == 0 || tmp_label.Text == "-")
            {
                this.is_sign = false;
                 tmp_label.Text = "0";
            }
        }
    }
}
