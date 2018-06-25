### 关于Redis 节点的配置

1. 配置内容

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