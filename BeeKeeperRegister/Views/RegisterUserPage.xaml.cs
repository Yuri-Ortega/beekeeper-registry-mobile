using BeeKeeperRegister.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;

namespace BeeKeeperRegister.Views;

public partial class RegisterUserPage : ContentPage
{
    public RegisterUserPage(RegisterUserViewModel vm)
	{
        InitializeComponent();
		BindingContext = vm;

        slideView.CurrentItemChanged += (s, e) =>
        {
            _= UpdateSteps();
        };
        _= UpdateSteps();
    }


    int currentStep = 0;

    async void OnNextClicked(object sender, EventArgs e)
    {
        if (BindingContext is RegisterUserViewModel vm)
        {
            vm.CurrentStep = currentStep;

            var isValid = vm.ValidateStep();

            if (!isValid)
                return;

            if (currentStep == 3)
            {
                await vm.RegisterAccountCommand.ExecuteAsync(null);
                await ResetStepperUI();
                return;
            }

                currentStep++;
                slideView.ShowNext();
                _ = UpdateSteps();

            if (currentStep == 3)
                nextButton.Content = "Finish";
        }
    }

    void OnBackClicked(object sender, EventArgs e)
    {
        if (currentStep > 0)
        {
            currentStep--;
            slideView.ShowPrevious();
            _= UpdateSteps();
        }

        nextButton.Content = "Next";
    }

     async Task UpdateSteps()
    {
        Color activeColor = Color.FromArgb("#698E49");
        Color inactiveColor = Color.FromArgb("#BDBDBD");

        Color activeTextColor = Color.FromArgb("#E0E0E0");
        Color inactiveTextColor = Color.FromArgb("#404040");

        step1.BackgroundColor = currentStep >= 0 ? activeColor : inactiveColor; 
        step1Txt.TextColor = currentStep >= 0 ? activeTextColor : inactiveTextColor;

        step2.BackgroundColor = currentStep >= 1 ? activeColor : inactiveColor;
        step2Txt.TextColor = currentStep >= 1 ? activeTextColor : inactiveTextColor;

        step3.BackgroundColor = currentStep >= 2 ? activeColor : inactiveColor;
        step3Txt.TextColor = currentStep >= 2 ? activeTextColor : inactiveTextColor;

        step4.BackgroundColor = currentStep >= 3 ? activeColor : inactiveColor;
        step4Txt.TextColor = currentStep >= 3 ? activeTextColor : inactiveTextColor;

        line1.BackgroundColor = currentStep >= 1 ? activeColor : inactiveColor;
        line2.BackgroundColor = currentStep >= 2 ? activeColor : inactiveColor;
        line3.BackgroundColor = currentStep >= 3 ? activeColor : inactiveColor;

        backButton.IsVisible = currentStep > 0;

        await backButton.FadeTo(backButton.IsEnabled ? 1 : 0.5, 200);
        await nextButton.FadeTo(nextButton.IsEnabled ? 1 : 0.5, 200);

        await AnimateStep(step1, currentStep == 0);
        await AnimateStep(step2, currentStep == 1);
        await AnimateStep(step3, currentStep == 2);
        await AnimateStep(step4, currentStep == 3);

        ScrollToCurrentStep();
    }

    async void ScrollToCurrentStep()
    {
        View target = currentStep switch
        {
            0 => step1,
            1 => step2,
            2 => step3,
            3 => step4,
            _ => step1
        };

        await stepScroll.ScrollToAsync(target, ScrollToPosition.Center, true);
    }

    async Task AnimateStep(View step, bool isActive)
    {
        if (isActive)
        {
            await step.ScaleTo(1.2, 150);
            await step.ScaleTo(1.0, 150);
        }
        else
        {
            await step.ScaleTo(1.0, 100);
        }
    }

    async Task ResetStepperUI()
    {
        while (currentStep > 0)
        {
            slideView.ShowPrevious();
            currentStep--;
            await Task.Delay(50);
        }

        nextButton.Content = "Next";

        await UpdateSteps();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RegisterUserViewModel vm)
        {
            await vm.LoaderAsync();
        }
    }
}
