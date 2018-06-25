### MutexMessage 主要是信号量的处理

1. 底层的支撑为---Redis

2. 集成功能为: 单账号登录(一个账号不能同时登录在多个设备上)

### 项目的依赖

1. Newtonsoft.Json
2. Microsoft.AspNet.SignalR (这个需要在Web项目上安装)
3. StackExchange.Redis

### 与其他系统集成操作代码

1. 配置Redis

```
<configSections>
	<section name="redisServer" type="MutexMessage.Internal.RedisConfig,MutexMessage"/>
</configSections>

<!--节点的具体内容-->
<redisServer>
	<servers name="server01" password="Aa123456" timeout="5" database="1">
		<server ip="127.0.0.1" port="6379" />
	</servers>
</redisServer>

```

2. Global.asax 中注册LoginChannel管道

```
protected void Application_Start()
{
	//租户类型与租户Id
	LoginChannel.ChannelInstance.Init("CTS","SLogin");
}

```
3. 在业务代码的登录操作处

```
var userModel = new UserRedisModel()
{
	UserName="ctspm",
	UserId="Abcd1234",
	FullName=""
};
LoginChannel.ChannelInstance.Login(userModel);

```

4. 在业务代码的登出操作处

```
var userModel = new UserRedisModel()
{
	UserName="",
	UserId="",
	FullName=""
};
LoginChannel.ChannelInstance.Logout(userModel);


```
5. 在业务代码处判断用户是否是最新登录的那一个

```
var userModel = new UserRedisModel()
{
	UserName="",
	UserId="",
	FullName=""
};
if(LoginChannel.ChannelInstance.IsCurrentUserReplaced(userModel))
{
	//当前的用户被替代了
}
```

6. 在前端_Layout.cshtml处

```
$(function () {
    var messageHub = $.connection.MessageHub;
    $.connection.hub.start().done(function () {
          messageHub.server.register();  
    });
    //定义登出提示信息
    messageHub.client.logout = function () {
        window.location = "http://www.wetrial.com/account/logout";//这里写上当前系统的登出地址
    };
});

```