### MutexMessage ��Ҫ���ź����Ĵ���

1. �ײ��֧��Ϊ---Redis

2. ���ɹ���Ϊ: ���˺ŵ�¼(һ���˺Ų���ͬʱ��¼�ڶ���豸��)

### ��Ŀ������

1. Newtonsoft.Json
2. Microsoft.AspNet.SignalR (�����Ҫ��Web��Ŀ�ϰ�װ)
3. StackExchange.Redis

### ������ϵͳ���ɲ�������

1. ����Redis

```
<configSections>
	<section name="redisServer" type="MutexMessage.Internal.RedisConfig,MutexMessage"/>
</configSections>

<!--�ڵ�ľ�������-->
<redisServer>
	<servers name="server01" password="Aa123456" timeout="5" database="1">
		<server ip="127.0.0.1" port="6379" />
	</servers>
</redisServer>

```

2. Global.asax ��ע��LoginChannel�ܵ�

```
protected void Application_Start()
{
	//�⻧�������⻧Id
	LoginChannel.ChannelInstance.Init("CTS","SLogin");
}

```
3. ��ҵ�����ĵ�¼������

```
var userModel = new UserRedisModel()
{
	UserName="ctspm",
	UserId="Abcd1234",
	FullName=""
};
LoginChannel.ChannelInstance.Login(userModel);

```

4. ��ҵ�����ĵǳ�������

```
var userModel = new UserRedisModel()
{
	UserName="",
	UserId="",
	FullName=""
};
LoginChannel.ChannelInstance.Logout(userModel);


```
5. ��ҵ����봦�ж��û��Ƿ������µ�¼����һ��

```
var userModel = new UserRedisModel()
{
	UserName="",
	UserId="",
	FullName=""
};
if(LoginChannel.ChannelInstance.IsCurrentUserReplaced(userModel))
{
	//��ǰ���û��������
}
```

6. ��ǰ��_Layout.cshtml��

```
$(function () {
    var messageHub = $.connection.MessageHub;
    $.connection.hub.start().done(function () {
          messageHub.server.register();  
    });
    //����ǳ���ʾ��Ϣ
    messageHub.client.logout = function () {
        window.location = "http://www.wetrial.com/account/logout";//����д�ϵ�ǰϵͳ�ĵǳ���ַ
    };
});

```