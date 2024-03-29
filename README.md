[View in English](./README_English.md)
# Utilities_Collections
小工具大杂烩 by HoraceHYY A.K.A. `MSFT_SuperFan`

要不要顺便来看看 [自由计算器安卓版](https://www.github.com/HoraceHuang-ui/FreeCalc_Android) 呀？
# 目录
- [开发说明](#开发说明)
- [安装说明](#安装说明)
- [使用说明](#使用说明)
  - [主页](#-主页)
  - [自由计算器](#-自由计算器)
  - [行列式计算器](#-行列式计算器)
  - [像素密度计算器](#-像素密度计算器)
  - [物理实验计算器](#-物理实验计算器)
  - [随机数生成器](#-随机数生成器)
  - [必应每日壁纸](#-必应每日壁纸)
  - [域名解析](#-域名解析)
  - [IP 地址查询](#-ip-地址查询)
  - [号码归属地](#-号码归属地)
  - [倒数日 Days Matter](#%EF%B8%8F-倒数日-days-matter)
  - [BV / av 号转换](#-bv--av-号转换)
  - [文本比较器](#-文本比较器)
  - [命令提示符](#%EF%B8%8F-命令提示符)
  - [设置页](#%EF%B8%8F-设置页)
- [其他说明](#其他说明)
- [代码说明](#代码说明)
## 开发说明
- 平台：`通用 Windows (UWP)`
- 目标系统版本：`Windows 11 (10.0 Build 22000)`
- 最低系统版本：`Windows 10 1809 (10.0 Build 17763)`
- 开发环境：`Visual Studio 2019 Community`
- 语言：前端 `XAML`  后端 `C#`
## 安装说明
- 自己 `Clone` 一下代码到本地编译呗 ~~开发说明都给你了这不是有手就行？~~
- 在 `Release` 中找到安装包并按接下来的教程安装 ~~但我还没写也懒得研究~~
## 使用说明
接下来将介绍小工具大杂烩所有的工具都有些啥功能
### 🏠 主页
> 一个软件连个主页都没有你能忍？？？？
### 🧮 自由计算器
> 好用的一批兄弟们！我自己都天天用拜托！功能多到批爆！自由度比按键计算器高多了！
- 支持的运算符：`+-*/^()`
- 支持的常量：
  - P: 圆周率 `3.1415926...`
  - E: 自然对数的底数 `2.7182818...`
- 支持的函数：
  - sqr: 平方根 `sqr9 = 3`
  - sin: 正弦 `sin45 = 0.707`
  - cos: 余弦 `cos45 = 0.707`
  - tan: 正切 `tan45 = 1`
  - cot: 余切 `cot45 = 1`
  - abs: 绝对值 `abs(-10) = 10`
  - cei: 向上取整 `cei4.3 = 5`
  - flo: 向下取整 `flo4.7 = 4`
  - log: 对数 `log(10,100) = 2`
- 支持记忆操作：
  - 当计算结果框显示时，`M+` 和 `M-` 操作将会直接取计算结果
  - 计算结果框不显示时，会计算出当前式子的结果并保存
  - `Copy` 键可以将记忆的数字复制进剪贴板
  - 在式子中直接输入 `M` 即可代入记忆值，等价于MR操作
- 在 `设置` 页中可以调整 `小数精确位数` 和 `角度/弧度制`
- 输入式子后按回车即可算出结果，在结果框中点一下即可将结果显示在左侧框内
- 左边输入框的内容被修改时结果框（若显示）会消失，再按回车即可再次出现
### 🔡 行列式计算器
> 可以计算一个n阶矩阵的行列式，对线代很有用
- 先输入矩阵阶数
- 然后 **依次** 输入矩阵的每一行
- 结果出来后在结果框内 **再按一次回车** 即可清空
### 💻 像素密度计算器
> 可以根据分辨率和屏幕尺寸以及子像素排列算出你屏幕的PPI，以此提供一个显示精细度的参考
- 输入屏幕的 `横向分辨率`、`纵向分辨率` 以及 `对角线尺寸` 即可计算出 `像素密度`
- **在对角线尺寸框内按回车** 以得到结果
- 支持 `RGB、Delta、周冬雨、PenTile、钻石排列`
- 文本框们的下方有多个分辨率及屏幕尺寸预设可以直接快速填入
### 🔬 物理实验计算器
> 你是否也在为物理实验要算不确定度而烦恼？用它就对了家人们！！！平均值、不确定度、相对不确定度一站式解决！
- 依次输入 `测量值` 即可，`平均值`、`不确定度` 和 `相对不确定度` 会 **实时更新**
- `测量值` 输入框左边的下拉栏是 `历史记录` 菜单，点击可查看你输入的 **所有测量值**
- 点击 `历史记录` 菜单中的某一项即可撤销那一项 **（不包含）** 后面的所有数据
- 在 `设置` 页中可以手动调整 `小数精确位数`，默认开启 `自动修约`
- 在 `设置` 页中还可以设置每次输入测量值 `保留整数部分` 以及小数点，每次只重新输入小数部分就好了，对一些 delta 较小的数据很有用；默认是按回车清空
### 🔀 随机数生成器
> 不多说了，班干部抽签必备，懂的都懂
- 输入 `最小值` 和 `最大值` 以确定生成数据范围，并设置 `生成数字数量`
- 勾选 `去除重复` 即可生成不重复的数字
- 点击 `生成随机数` 即可在 `结果` 框中显示
- 所有数据 **升序** 排列，中间以 `, `  分隔
### 🌄 必应每日壁纸
> 真的很好看！简直就是美呆了！
- 默认显示 **当天、中国区** 的必应壁纸
- `调整大小` 是个条，可以调整图片大小，默认值在 50；也可以直接用触控板手势或鼠标调整
- 标题栏右边的 `i` 图标可显示图片介绍及版权说明
- `保存图片` 按钮可在浏览器中查看图片原图，此时 **右击** 图片然后选择 `保存图片` 即可存到本地
- 图片右上角的 `翻页` 按钮可以查看历史图片，最多可以查看到 **8 天前**
- 设置中可以调整 `地区`，选项有 `中国、美国、英国、新西兰、澳大利亚、日本、德国`
- 设置中还有 `分辨率` 选项，可设置 `768p、1080p、2160p`
### 🌐 域名解析
> 我自己是没啥用啦但我同学要用，反正也不难做，调用个 API 的事情
- 感谢 [天行数据](https://www.tianapi.com/) 提供的 API 服务
- **重要** 所有设备加起来每天只能使用 `1000` 次！！！
- 输入一个 `域名`，如： `baidu.com`，即可解析出其对应的 `IP 地址` 以及 `存活时间、类型` 等详细信息
### 🔗 IP 地址查询
> 我自己是没啥用啦但我同学要用，反正也不难做，调用个 API 的事情
- 感谢 [天行数据](https://www.tianapi.com/) 提供的 API 服务
- **重要** 所有设备加起来每天只能使用 `1000` 次！！！
- 输入一个 `IP 地址`，即可解析出其对应的 `地区、运营商` 等详细信息
### 📞 号码归属地
> 我也不知道有啥用不过想到了就做了，反正也不难做，调用个 API 的事情
- 感谢 [天行数据](https://www.tianapi.com/) 提供的 API 服务
- **重要** 所有设备加起来每天只能使用 `1000` 次！！！
- 输入一个 **中国的** `电话号码`，即可解析出其对应的 `地区、运营商` 等详细信息
### 🗓️ 倒数日 Days Matter
> 可以帮你记住一些重要的事情，并告诉你已经过去或还剩几天！ ~~距离我上次更视频已经过去 38 天了 [欢迎关注我](https://space.bilibili.com/66003595)~~
- ~~笑死，根本没做~~
- ~~笑死，根本不会写~~
- 但是我以后会写的！！！！
### 📺 BV / av 号转换
> 有些人发了个 BV 号你没见过，点进去才发现是那个熟悉的视频？这个来帮你刻入新 DNA
- 本工具包含三个要素：`BV 号`、`av 号`、`视频链接`。输入任意一个都可生成另外两个
- `BV 号` 和 `av 号` 框内可不输入“BV”或者“av”，直接输入编号即可自动加上并识别
### 📄 文本比较器
> 在左右两侧分别输入要比较的文本，两者的区别即可由色块标示
- ~~笑死，根本不会写~~
- 我研究了好久也不会写！！！！！
- 但是在主页留了个入口，就挖坑呗
- UI 也已经写好了，就是不知道怎么实现
### ⌨️ 命令提示符
> 我也不知道我当初写这个是为了干啥，屁用没有，但是写都写了干脆放着，删了怪可惜的
- 只支持单条命令，两次输入之间不能联系，不支持 `ADB` 等依赖环境变量的命令
- 有时候拿来看个 `ipconfig` 勉强能用？
### ⚙️ 设置页
> 除了上面提到的这些选项外，还有：
- **`通用` 分类中的选项改了需要重启应用生效！**
- `显示模式`：默认为跟随系统，也可以手动选择亮暗色
- `语言`：支持中英切换
- `导航栏位置`：可选择将导航栏放在左侧或顶部 ~~但是顶部又不好看又不方便啊，谁会用？~~
- 上面那三个用了 `天行数据API` 的功能可以在这里统一更改 `apikey`，默认是用我的
## 其他说明
- 本程序会在用户文件夹 `Documents\utilities` 目录下生成文件存储设置偏好，文件名为 `datav版本号.txt`
- 每次新版本都会生成一个新的txt，以此判断要不要开屏弹一个更新内容
- 如果你 ~~闲的蛋疼~~ 比较硬核的话可以直接改txt的内容来修改设置，使用 `JSON` 的语法
- 希望优化或增加功能的话欢迎 [提issue](https://github.com/HoraceHuang-ui/Utilities_Collections/issues) ，上面说不会写的两个功能我也自己提了 issue，欢迎热心大佬来给自己 Assign 一下
- 如果你技术力溢出或者大发慈悲想帮帮我完善项目的话欢迎创建一个 `Branch` 或者 `Fork` 一下并向我提交 PR！！！感激不尽！！！！！
## 代码说明
> 呃呃呃，还要我告诉你代码结构怎么样的吗？~~我写的代码乱得像一座屎山我自己都不想看~~ 这我需要好好组织一下语言了
- To be continued...
- 我以后 ~~闲的没事干~~ 时间允许的话会写点的
## 感谢看完！
- [一键回到顶部](https://github.com/HoraceHuang-ui/Utilities_Collections)
