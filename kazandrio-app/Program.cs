using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using static OpenQA.Selenium.BiDi.Modules.Script.RealmInfo;

Console.WriteLine("Appium testi başlatılıyor...");

var options = new AppiumOptions();
options.PlatformName = "Android";
options.DeviceName = GetConnectedDeviceId(); // Otomatik alınıyor
options.PlatformVersion = "10.0";
options.AutomationName = "UiAutomator2";
options.AddAdditionalAppiumOption("appPackage", "com.pepsico.kazandirio");
options.AddAdditionalAppiumOption("appActivity", ".MainActivity");
options.AddAdditionalAppiumOption("noReset", true);
options.AddAdditionalAppiumOption("androidHome", "C:\\Android\\sdk");


AndroidDriver driver = new AndroidDriver(
    new Uri("http://192.168.1.103:4723/"), // <-- DİKKAT! /wd/hub YOK
    options
);

IWebElement okutKazanButton = driver.FindElement(By.Id("com.pepsico.kazandirio:id/navigation_bar_item_labels_group"));
okutKazanButton.Click();

IWebElement toolbarTitle = driver.FindElement(By.Id("com.pepsico.kazandirio:id/text_view_toolbar_title"));
toolbarTitle.Click();

IWebElement passwordField = driver.FindElement(By.Id("com.pepsico.kazandirio:id/text_view_manual_code_write_enter_code"));
passwordField.SendKeys("12345678910");

IWebElement tamamButton = driver.FindElement(By.Id("com.pepsico.kazandirio:id/button_manual_code_write_ok"));
tamamButton.Click();

Console.WriteLine("Şifre başarıyla girildi.");

driver.Quit();

 static string GetConnectedDeviceId()
{
    var adbPath = @"C:\platform-tools\adb.exe";

    ProcessStartInfo psi = new ProcessStartInfo
    {
        FileName = adbPath,
        Arguments = "devices",
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using (Process process = Process.Start(psi))
    {
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // İlk satırı atla, çünkü başlık oluyor: "List of devices attached"
        foreach (var line in lines.Skip(1))
        {
            if (line.Contains("\tdevice"))
            {
                return line.Split('\t')[0];
            }
        }
    }

    return null; // Cihaz yoksa
}