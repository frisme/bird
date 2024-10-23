namespace bird;

public partial class GamePage : ContentPage
{

	double HeightWindow = 0;
	double WidthtWindow = 0;
	const int Velocity = 15;
	const int Gravity = 5;
	

	const int TimeBeteweenFrames = 25;

	bool IsDied = true;
	const int JumpForce = 35;
	const int maxJumpTime = 5;
	bool IsJumping = false;
	int JumpTime = 0;
	const int minOpening = 100;
	int score = 0;

	// Ai


	public GamePage()
	{
		InitializeComponent();
	}

	bool VerifyColision()
	{
		if (VerifyColisionT() ||
		VerifyColisionC() ||
		VerifyColisionCanoCima() ||
		VerifyColisionCanoBaixo())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	bool VerifyColisionT()
	{
		var minY = -HeightWindow / 2;
		if (Passaro.TranslationY <= minY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool VerifyColisionC()
	{
		var maxY = HeightWindow / 2 - Chao.HeightRequest - 37;
		if (Passaro.TranslationY >= maxY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool VerifyColisionCanoCima()
	{
		var posHPassaro = (WidthtWindow/2) - (Passaro.WidthRequest/2);
		var posVPassaro = (HeightWindow/2) - (Passaro.HeightRequest/2) + Passaro.TranslationY;
		var yMaxCano = fenoCima.HeightRequest + fenoCima.TranslationY + minOpening;
		if(posHPassaro >= Math.Abs(fenoCima.TranslationX) - fenoCima.WidthRequest &&
		   posHPassaro >= Math.Abs(fenoCima.TranslationX) + fenoCima.WidthRequest &&
		   posVPassaro <= fenoCima.HeightRequest + fenoCima.TranslationY)
		{
			return true;
		}
		else
		{
			return false;
		}

	}

	bool VerifyColisionCanoBaixo()
	{
		var posHPassaro = (WidthtWindow/2) - (Passaro.WidthRequest/2);
		var posVPassaro = (HeightWindow/2) - (Passaro.HeightRequest/2) + Passaro.TranslationY;
		var yMaxCano = fenoBaixo.HeightRequest + fenoBaixo.TranslationY + minOpening;
		if(posHPassaro >= Math.Abs(fenoBaixo.TranslationX) - fenoBaixo.WidthRequest &&
		   posHPassaro <= Math.Abs(fenoBaixo.TranslationX) + fenoBaixo.WidthRequest &&
		   posVPassaro <= fenoBaixo.HeightRequest + fenoBaixo.TranslationY)
		{
			return true;
		}
		else
		{
			return false;
		}

	}

	void ApplyGravity()
	{
		Passaro.TranslationY += Gravity;
		
	}
	void ApplyJump()
	{
		Passaro.TranslationY -= JumpForce;
		JumpTime++;
		if (JumpTime >= maxJumpTime)
		{
			IsJumping = false;
			JumpTime = 0;
		}
	}

	public async void Desenha()
	{
		while (!IsDied)
		{
			if (IsJumping)
			{
				ApplyJump();
			}
			else 
			{
				ApplyGravity();
			}
			ApplyGravity();
			ManageTower();
			if (VerifyColision())
			{
				IsDied = true;
				GameOverFrame.IsVisible = true;
				break;
			}
			await Task.Delay(TimeBeteweenFrames);
		}
	}
	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		WidthtWindow = width;
		HeightWindow = height;
	}

	void ManageTower()
	{
		fenoBaixo.TranslationX -= Velocity;
		fenoCima.TranslationX -= Velocity;


		if (fenoBaixo.TranslationX < -WidthtWindow)
		{
			fenoBaixo.TranslationX = 0;
			fenoCima.TranslationX = 0;
			var MaxHeight = -100;
			var MinHeight = -fenoBaixo.HeightRequest;
			fenoCima.TranslationY = Random.Shared.Next((int)MinHeight, (int)MaxHeight);
			fenoBaixo.TranslationY = fenoCima.TranslationY + minOpening + fenoBaixo.HeightRequest;
			score++;
			LabelScore.Text = "Canos:" + score.ToString("D3");
		}
	}





	void Ai(Object s, TappedEventArgs e)
	{
		GameOverFrame.IsVisible = false;
		IsDied = false;
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{
		Passaro.TranslationY = 0;
		fenoCima.TranslationX = - WidthtWindow;
		fenoBaixo.TranslationY = - WidthtWindow;
		Passaro.TranslationX = 0;
		Passaro.TranslationY = 0;
		score = 0;
	}

	void Ui (Object s, TappedEventArgs e)
	{
		IsJumping = true;
	}
}