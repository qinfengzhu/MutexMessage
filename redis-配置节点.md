### ����Redis �ڵ������

1. ��������

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