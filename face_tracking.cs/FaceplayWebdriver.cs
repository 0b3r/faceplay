using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;


class Webdriver
{
    IWebDriver driver;
    IJavaScriptExecutor jse;

    public Webdriver(String s)
    {
        driver = new FirefoxDriver();
        driver.Navigate().GoToUrl(s);          
        jse = (IJavaScriptExecutor)driver;
    }

    IAction enterAction = null;
    public void Enter()
    {
        if (enterAction == null)
        {
            Actions achain = new Actions(driver);
            achain.SendKeys(Keys.Enter);
            enterAction = achain.Build();
        }
        enterAction.Perform();
    }

    IAction tabForwardAction = null;
    public void TabForward()
    {
        if (tabForwardAction == null)
        {   
            Actions achain = new Actions(driver);
            achain.SendKeys(Keys.Tab);
            tabForwardAction = achain.Build();
        }
        tabForwardAction.Perform();
    }

    IAction tabBackwardAction = null;
    public void TabBackward()
    {
        if (tabBackwardAction == null)
        {
            Actions achain = new Actions(driver);
            achain.KeyDown(Keys.Shift).SendKeys(Keys.Tab).KeyUp(Keys.Shift);
            tabBackwardAction = achain.Build();
        }
        tabBackwardAction.Perform();
    }

    public void ScrollUp() 
    {
        jse.ExecuteScript("window.scrollBy(0,-100)");
    }

    public void ScrollDown() 
    {
        jse.ExecuteScript("window.scrollBy(0,100)");
    }

    IAction focusLocationBarAction = null;
    public void FocusLocationBar() 
    {
        if (focusLocationBarAction == null)
        {
            Actions achain = new Actions(driver);
            achain.KeyDown(Keys.Control).SendKeys("l").KeyUp(Keys.Control);
            focusLocationBarAction = achain.Build();
        }
        focusLocationBarAction.Perform();
    }
}
