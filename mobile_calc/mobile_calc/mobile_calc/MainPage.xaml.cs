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

        /// <summary>
        /// class memeber init
        /// </summary>
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

        /// <summary>
        /// 숫자 버튼 이벤트
        /// 해당 버튼의 숫자를 tmp_label에 입력한다.
        /// </summary>
        /// <param name="sender">이벤트 발생 대상</param>
        /// <param name="e">이벤트 종류</param>
        private void Num_Clicked(object sender, EventArgs e)
        {
            // tmp_label.Text가 "0"이거나 MainPage.is_result가 ture일 때
            if (tmp_label.Text == "0" || this.is_result)
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

            // tmp_label.Text가 "-0"일 때
            else if (tmp_label.Text=="-0")
            {
                tmp_label.Text = "-";
            }

            // 숫자 버튼의 해당하는 수를 tmp_label에 추가한다.
            tmp_label.Text = tmp_label.Text + ((Button)sender).Text;
        }

        /// <summary>
        /// 소수점 버튼 이벤트
        /// -소수점이 없을 때
        ///     * tmp_label.Text 맨 마지막에 소수점을 추가한다.
        /// -소수점이 있을 때
        ///     * tmp_label.Text 맨 마지막에 소수점이 있으면 삭제
        /// </summary>
        /// <param name="sender">이벤트 발생 대상</param>
        /// <param name="e">이벤트 종류</param>
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
            }
        }

        /// <summary>
        /// 부호(±) 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sign_Clicked(object sender, EventArgs e)
        {
            // MainPage.is_sign이 false일 때
            // is_sign을 true로 설정하고 tmp_label.Text의 맨 앞에 "-"를 추가한다.
            if (this.is_sign == false)
            {
                this.is_sign = true;
                tmp_label.Text = "-" + tmp_label.Text;
            }

            // MainPage.is_result가 false가 아닐 때
            // MainPage.is_sign을 false로 설정하고 tmp_label의 맨 앞의 문자를 제거한다.
            else
            {
                this.is_sign = false;
                tmp_label.Text = tmp_label.Text.Substring(1);
            }

            // MainPage.is_result가 true일 때
            // MainPage.is_result, is_sign을 각각 false, true로 설정하고
            // tmp_label.Text를 "-0"으로 설정한다.
            if (this.is_result == true)
            {
                this.is_result = false;
                this.is_sign = true;
                tmp_label.Text = "-0";
            }
        }

        /// <summary>
        /// 초기화 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Clicked(object sender, EventArgs e)
        {
            this.Init();
            polynomial_label.Text = "";
            tmp_label.Text = "0";
        }

        /// <summary>
        /// 입력 정보 초기화 이벤트
        /// 현재 입력된 숫자에 대해서 초기화 된다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Entry_Clicked(object sender, EventArgs e)
        {
            this.Init();
            tmp_label.Text = "0";
        }

        /// <summary>
        /// 사칙연산 버튼 이벤트
        /// 연속적인 계산을 지원한다.
        /// </summary>
        /// <example>
        /// 15 + 45 + 26 + ... + 입력된 숫자
        /// </example>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


            // MainPage.is_last가 false이고 is_first가 true일 때 
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

            // MainPage.is_last, is_first가 false일 때
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

                // MainPage.tmp_result의 값이 옳바른 값일 때
                if (this.calc.Is_Right(this.tmp_result))
                {
                    polynomial_label.Text = polynomial_label.Text + tmp_label.Text + this.tmp_operator;
                    tmp_label.Text = Convert.ToString(this.tmp_result);
                }

                // MainPage.tmp_result의 값이 옳바른 값이 아닐 때
                else
                {
                    this.Init();
                    polynomial_label.Text = "";
                    tmp_label.Text = "0으로 나눌 수 없습니다.";
                }

            }

            // 다음 피연산자의 입력을 위해 is_sign, is_point 설정을 해제한다.
            this.is_sign = false;
            this.is_point = false;
        }

        /// <summary>
        /// 결과(=) 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Result_Clicked(object sender, EventArgs e)
        {
            // MainPage.is_first가 true일 때
            if (this.is_first == true)
            {
                return;
            }

            // MainPage.is_first가 false일 때
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

        /// <summary>
        /// 뒤로가기 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Clicked(object sender, EventArgs e)
        {
            // tmp_label.Text의 맨 마지막이 "."일 때
            if (tmp_label.Text.LastIndexOf('.') == tmp_label.Text.Length - 1)
            {
                this.is_point = false;
            }

            // tmp_label.Text를 tmp_label.Text 맨 마지막을 뺀 문자열로 설정한다.
            tmp_label.Text = tmp_label.Text.Substring(0, tmp_label.Text.Length - 1);

            // is_sign 을 해제하고 tmp_label을 "0"으로 설정한다.
            // tmp_label.Text의 길이가 0이거나 tmp_label.Text가 "-"일 때
            if (tmp_label.Text.Length == 0 || tmp_label.Text == "-")
            {
                this.is_sign = false;
                 tmp_label.Text = "0";
            }
        }
    }
}
