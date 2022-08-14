[查看中文版文档](./README.md)
# Utilities_Collections
Utilities Collections by HoraceHYY A.K.A `MSFT_SuperFan`

Also check out [FreeCalc for Android](https://www.github.com/Horacehuang-ui/FreeCalc_Android) !
# Contents
- [Development Info](#development-info)
- [Installtion Guides](#installation-guides)
- [Usage Guides](#usage-guides)
  - [Home](#-home)
  - [Free Calculator](#-free-calculator)
  - [Determinant Calculator](#-determinant-calculator)
  - [Pixel Density Calculator](#-pixel-density-calculator)
  - [Physics Experiment Calculator](#-physics-experiment-calculator)
  - [Number Randomizer](#-number-randomizer)
  - [Bing Daily Wallpapers](#-bing-daily-wallpapers)
  - [Domain Analyzer](#-domain-analyzer)
  - [IP Request](#-ip-request)
  - [Number Region](#-number-region)
  - [Days Matter](#-days-matter)
  - [BV / av Converter](#-bv--av-converter)
  - [Text Comparator](#-text-comparator)
  - [Command Prompt](#-command-prompt)
  - [Settings Page](#-settings-page)
- [Other Info](#other-info)
- [Coding Info](#coding-info)
## Development Info
- Platform: `Universal Windows (UWP)`
- Target Windows SDK: `Windows 11 (10.0 Build 22000)`
- Min Windows SDK: `Windows 10 1809 (10.0 Build 17763)`
- IDE: `Visual Studio 2019 Community`
- Language: Frontend `XAML` ; Backend `C#`
## Installation Guides
- `Clone` the repo and build one yourself. ~~It isn't that hard bro~~
- Get the package from `Releases` and follow the steps below ~~though I haven't worked on it~~
## Usage Guides
All the features given to you by Utilities Collections are listed below.
### 🏠 Home
> Have you ever seen any app that doesn't have a homepage???
### 🧮 Free Calculator
> It contains a whole bunch of useful features that'll maximize your efficiency.
- Operators: `+-*/^()`
- Constants:
  - P: Pi `3.1415926...`
  - E: Euler's `2.7182818...`
- Functions:
  - sqr: Square root `sqr9 = 3`
  - sin: Sine `sin45 = 0.707`
  - cos: Cosine `cos45 = 0.707`
  - tan: Tangent `tan45 = 1`
  - cot: Cotangent `cot45 = 1`
  - abs: Absolute `abs(-10) = 10`
  - cei: Ceil `cei4.3 = 5`
  - flo: Floor `flo4.7 = 4`
  - log: Logarithm `log(10,100) = 2`
- Memory Features:
  - When the result box is displayed, `M+` and `M-` operations will save the data inside directly.
  - When it is not, they calculate what's in the expression box and do memory operations.
  - `Copy` button copies the data in memory to Clipboard.
  - Implement an `M` in your expression to do an `MR` operation.
- `Decimal Accuracy` and `DEG/RAD` options are available in `Settings` page.
- Press Enter to calculate what you've entered. Simply tap on the result box to move the result into the expression box.
- When the expression is changed, the result box (if displayed) disappears. Press Enter again to get it back.
### 🔡 Determinant Calculator
> It calculates the determinant of an N-dimensional matrix, which is useful for Linear Algebra.
- First enter the dimension of the matrix.
- Then enter **each** row of the matrix one by one.
- **Press Enter again** in the result box to clear.
### 💻 Pixel Density Calculator
> It calculates the PPI (Pixel Per Inch) value of your screen, resolution and size provided.
- Enter the `Horizontal` and `Vertical Resolution` as well as `Diagonal Size` to get the `PPI` value.
- **Press Enter in Diagonal Size** to display the result.
- Supports `RGB, Delta, BOE Delta, PenTile, Diamonds`
- There are several resolution and ratio presets to quickly enter the corresponding values.
### 🔬 Physics Experiment Calculator
> Have you ever been f\*\*cked by calculating uncertainty values in Physics? Then you shouldn't miss this! Average, uncertainty and relative uncertainty, all in one!
- Enter the measured `Data` repeatedly, and the `Average`, `Uncertainty` and `Relative Uncertainty` values are **updated real-time**.
- The combo box on the left of `Data` displays what you've entered, tap to view `History` values.
- Tap on an item in `History` menu to delete the datas after that **(without the one you've tapped)**.
- You can `have the integer part kept` as you enter the datas. This feature is useful when the delta between values are relatively small. By default, the whole box is cleared as you press Enter.
### 🔀 Number Randomizer
> Very useful when it's hard to decide who is to do a job :)
- Input the `Max` and `Min` value and set `How Many` numbers are to be generated.
- Enable `No Duplicates` to avoid any repeated numbers.
- Press `Randomize` to get the result.
- All values are displayed by an **ascending** order, separated by `, ` .
### 🌄 Bing Daily Wallpapers
> DAMN BEAUTIFUL!!! BREATHTAKING!!!
- Displays wallpapers distributed in **Chinese market, the same day**.
- `Resize` provides a slider for you, and is set to 50/100 by default. You can also resize the image using a trackpad or mouse.
- The `i` button on the right of the title bar displays the description and copyright.
- `Save Image` button takes you to your web browser and display the original image. Right-click on it and tap `Save Image` to download.
- The `Left/Right Arrows` on the upper-right corner lets you manipulate through images **from today to 8 days ago**.
- You can switch the market between `China, US, UK, New Zealand, Australia, Japan and Germany` in `Settings` page.
- In `Settings` you can also change the resolution of the image. There are `720p, 1080p and 2160p` available.
### 🌐 Domain Analyzer
> Well I don't use it much, but one of my friends wants this. So I did it.
- Warm thanks to [TianAPI](https://www.tianapi.com/) which provides the API services.
- **\*\*IMPORTANT\*\*** Available at most `1000` times per day, all users altogether!!!
- Provided a `URL`, e.g. `baidu.com`, and it analyzes its `IP Address` and more infos like `TTL, Type`.
### 🔗 IP Request
> Well I don't use it much, but one of my friends wants this. So I did it.
- Warm thanks to [TianAPI](https://www.tianapi.com/) which provides the API services.
- **\*\*IMPORTANT\*\*** Available at most `1000` times per day, all users altogether!!!
- Provided an `IP Address`, and it analyzes its infos like `Region, Provider`.
### 📞 Number Region
> Well I don't know how I can make use of it, but I just thought of this idea. So I did it.
- Warm thanks to [TianAPI](https://www.tianapi.com/) which provides the API services.
- **\*\*IMPORTANT\*\*** Available at most `1000` times per day, all users altogether!!!
- Provided a **Chinese** `Phone Number`, and it analyzes its infos like `Region, Provider`.
### 🗓️ Days Matter
> Helps you memorize some important things, and tells you how many days (have passed/is left). ~~39 days have passed since the last time I upload a video on Bilibili [Subscribe](https://spacce.bilibili.com/66003595)~~
- ~~Well I haven't started writing~~
- ~~Well I even don't know how to do this~~
- But I will try in the future!!!
### 📺 BV / av Converter
> Converts what you've deeply memorized to the new BV format.
- There are 3 factors: `BV`, `av` and `URL`. Any one of them can be converted to the other two.
- "BV" or "av" prefixes are not necessary in `BV` and `av` boxes. They will be added automatically.
### 📄 Text Comparator
> Enter two texts on both sides and it highlights the different parts between them.
- ~~Well I don't know how to do this...~~
- ...even after a bunch of time trying to figure this out!
- But I've finished UI design.
### ⌨️ Command Prompt
> Totally meaningless. But after all I've made it, so I decide to keep it.
- Only supports a single command each time, and it doesn't memorize data.
- Commands relying on Environment Variables aren't supported, e.g. `ADB`.
- Maybe sometimes checking your `ipconfig` is useful?
### ⚙️ Settings Page
> It also provides the following options besides what have been mentioned before:
- **Changes made to the `General` part take effect after restarting the app!**
- `Theme`: It follows system settings by default, or you can manually set it to light or dark.
- `Language`: Chinese and English available.
- `Navigation Pane Placement`: Place the Navigation Pane on the left or on the top. ~~But who will place it on the top???~~
- The `TianAPI`-powered features needs an `apikey`. They use mine by default, but feel free to change it to your own.
## Other Info
- The app creates files under `Documents\utilities` to store your preferences data. Files are named by `datav[Version].txt`.
- Each version generates a new txt file. They are used to determine whether to display a pop-up window introducing new features.
- If you are hardcore enough you can directly edit the txt file in `JSON` format to change your settings.
- You are welcome to [issue](https://github.com/HoraceHuang-ui/Utilities_Collections/issues) this repo if you got any idea! I've also opened corresponding issues for the two features that I can't do.
- Also if you want to contribute to this repo (I'll be very thankful for that) please `Branch` or `Fork` and PR!!! THANKS!!!
## Coding Info
> The code structures ~~is like a mountain of sh\*t~~ is a bit hard to describe...
- I'll write some if available :)
- To be continued...
## Thank you for reading!
- [Back to top](./README_English.md)
