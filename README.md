# dotnetcore-mnist说明
效果可见https://www.wang-yueyang.com/mnist 
* ConvNetSharp.Core和ConvNetSharp.Volume源自C#深度学习的框架 [ConvNetSharp](https://github.com/cbovar/ConvNetSharp)。<br/>
* DotNetCoreMnist基于.net core，项目基于[ConvNetSharp](https://github.com/cbovar/ConvNetSharp)的mnist数据预处理和训练，小改自[ConvNetSharp](https://github.com/cbovar/ConvNetSharp)例子[MnistDemo](https://github.com/cbovar/ConvNetSharp/tree/master/Examples/MnistDemo)，最终会把训练的结果上传至webapi服务。<br/>
**注意:** <br/>
若要使用，请修改上传(查询)的url，服务端代码于项目MnistWeb中。<br>
* Mnist是训练测试数据集，国内下载有时网络不稳定造成下载不全，故上传。<br/>
* MnistWeb是基于.net core的webapi服务，包括预测、保存训练集等服务，其中包括的静态html网页基于[cropperjs](https://github.com/fengyuanchen/cropperjs)的例子。<br/>
**注意:** <br/>
其使用mysql存储训练集，如要使用请修改appsettings.json中ConnectionStrings数据库连接字符串.<br/>
其使用DBFirst，使用前请先确保先创建数据库mnist及表net，sql如下:
```sql
CREATE TABLE `net` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `NetText` mediumtext,
  PRIMARY KEY (`Id`)
)
```
* UnitTest为单元测试，可忽略。<br/>
## 数据有时需要把截图调整到适当的位置才能正确识别，是由于训练集不够丰富所造成的，请不要惊慌。
