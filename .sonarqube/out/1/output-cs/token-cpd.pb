�

XC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\MessagingService\Contracts\Item.cs
	namespace 	
MessagingService
 
. 
	Contracts $
{ 
public 

class 
Item 
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
public 
decimal 
Price 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
Image 
{ 
get !
;! "
set# &
;& '
}( )
public

 
override

 
string

 
ToString

 '
(

' (
)

( )
{ 	
return 
$str 
+ 
Id 
+  
$str! +
+, -
(. /
Name/ 3
??4 6
$str7 9
)9 :
+; <
$str= H
+I J
PriceK P
.P Q
ToStringQ Y
(Y Z
)Z [
;[ \
}
} 
} �
SC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\MessagingService\IMQClient.cs
	namespace 	
MessagingService
 
{ 
public 

	interface 
	IMQClient 
{ 
void "
SubscribeToItemUpdates
(# $
Func$ (
<( )
Item) -
,- .
bool/ 3
>3 4
action5 ;
); <
;< =
void 
PublishItemUpdated
(  
Item  $
item% )
)) *
;* +
void		 "
SubscribeToItemDeletes		
(		# $
Func		$ (
<		( )
int		) ,
,		, -
bool		. 2
>		2 3
action		4 :
)		: ;
;		; <
void

 
PublishItemDeleted


(

  
int

  #
id

$ &
)

& '
;

' (
} 
} �M
XC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\MessagingService\RabbitMQClient.cs
	namespace 	
MessagingService
 
{ 
public		 

class		 
RabbitMQClient		 
:		  !
	IMQClient		" +
{

 
private 
readonly 
string 
exchangeName  ,
=- .
$str/ 6
;6 7
private 
readonly 
string  
itemUpdateRoutingKey  4
=5 6
$str7 E
;E F
private
readonly
string
itemDeleteRoutingKey
=
$str
;
private 
readonly 
IConnection $
_connection% 0
;0 1
private 
readonly 
IModel 
_channel  (
;( )
public 
RabbitMQClient 
( 
IConnection )

connection* 4
)4 5
{ 	
_connection 
= 

connection $
;$ %
_connection 
. 
ConnectionShutdown *
+=+ -)
Connection_ConnectionShutdown. K
;K L
_channel 
= 
_connection "
." #
CreateModel# .
(. /
)/ 0
;0 1
} 	
private 
void )
Connection_ConnectionShutdown 2
(2 3
object3 9
?9 :
sender; A
,A B
ShutdownEventArgsC T
eU V
)V W
{ 	
Console 
. 
	WriteLine 
( 
$str C
)C D
;D E
} 	
public 
void 
PublishItemDeleted &
(& '
int' *
id+ -
)- .
{ 	
_channel 
. 
ExchangeDeclare $
($ %
exchangeName% 1
,1 2
ExchangeType3 ?
.? @
Topic@ E
)E F
;F G
var   
itemToDelete   
=   
new   "
Item  # '
(  ' (
)  ( )
{  * +
Id  , .
=  / 0
id  1 3
}  4 5
;  5 6
var!! 
message!! 
=!! 
JsonSerializer!! (
.!!( )
	Serialize!!) 2
(!!2 3
itemToDelete!!3 ?
)!!? @
;!!@ A
var"" 
body"" 
="" 
Encoding"" 
.""  
UTF8""  $
.""$ %
GetBytes""% -
(""- .
message"". 5
)""5 6
;""6 7
_channel## 
.## 
BasicPublish## !
(##! "
exchangeName##" .
,##. / 
itemDeleteRoutingKey##0 D
,##D E
null##F J
,##J K
body##L P
)##P Q
;##Q R
}$$ 	
public%% 
void%% 
PublishItemUpdated%% &
(%%& '
Item%%' +
item%%, 0
)%%0 1
{&& 	
_channel'' 
.'' 
ExchangeDeclare'' $
(''$ %
exchangeName''% 1
,''1 2
ExchangeType''3 ?
.''? @
Topic''@ E
)''E F
;''F G
var(( 
message(( 
=(( 
JsonSerializer(( (
.((( )
	Serialize(() 2
(((2 3
item((3 7
)((7 8
;((8 9
var)) 
body)) 
=)) 
Encoding)) 
.))  
UTF8))  $
.))$ %
GetBytes))% -
())- .
message)). 5
)))5 6
;))6 7
_channel** 
.** 
BasicPublish** !
(**! "
exchangeName**" .
,**. / 
itemUpdateRoutingKey**0 D
,**D E
null**F J
,**J K
body**L P
)**P Q
;**Q R
}++ 	
public,, 
void,, "
SubscribeToItemDeletes,, *
(,,* +
Func,,+ /
<,,/ 0
int,,0 3
,,,3 4
bool,,5 9
>,,9 :
action,,; A
),,A B
{-- 	
_channel.. 
... 
ExchangeDeclare.. $
(..$ %
exchangeName..% 1
,..1 2
ExchangeType..3 ?
...? @
Topic..@ E
)..E F
;..F G
var// 
	queueName// 
=// 
_channel// $
.//$ %
QueueDeclare//% 1
(//1 2
)//2 3
.//3 4
	QueueName//4 =
;//= >
_channel00 
.00 
	QueueBind00 
(00 
	queueName00 (
,00( )
exchangeName00* 6
,006 7 
itemDeleteRoutingKey008 L
)00L M
;00M N
var22 
consumer22 
=22 
new22 !
EventingBasicConsumer22 4
(224 5
_channel225 =
)22= >
;22> ?
consumer33 
.33 
Received33 
+=33  
(33! "
sender33" (
,33( )
ea33* ,
)33, -
=>33. 0
{44 
var55 
body55 
=55 
ea55 
.55 
Body55 "
.55" #
ToArray55# *
(55* +
)55+ ,
;55, -
var66 
message66 
=66 
Encoding66 &
.66& '
UTF866' +
.66+ ,
	GetString66, 5
(665 6
body666 :
)66: ;
;66; <
var77 
item77 
=77 
JsonSerializer77 )
.77) *
Deserialize77* 5
<775 6
Item776 :
>77: ;
(77; <
message77< C
)77C D
;77D E
var88 
result88 
=88 
action88 #
.88# $
Invoke88$ *
(88* +
item88+ /
.88/ 0
Id880 2
)882 3
;883 4
if99 
(99 
result99 
)99 
_channel:: 
.:: 
BasicAck:: %
(::% &
ea::& (
.::( )
DeliveryTag::) 4
,::4 5
false::6 ;
)::; <
;::< =
else;; 
{<< 
}>> 
}?? 
;??
_channel@@ 
.@@ 
BasicConsume@@ !
(@@! "
	queueName@@" +
,@@+ ,
false@@- 2
,@@2 3
consumer@@4 <
)@@< =
;@@= >
}AA 	
publicBB 
voidBB "
SubscribeToItemUpdatesBB *
(BB* +
FuncBB+ /
<BB/ 0
ItemBB0 4
,BB4 5
boolBB6 :
>BB: ;
actionBB< B
)BBB C
{CC 	
_channelDD 
.DD 
ExchangeDeclareDD $
(DD$ %
exchangeNameDD% 1
,DD1 2
ExchangeTypeDD3 ?
.DD? @
TopicDD@ E
)DDE F
;DDF G
varEE 
	queueNameEE 
=EE 
_channelEE $
.EE$ %
QueueDeclareEE% 1
(EE1 2
)EE2 3
.EE3 4
	QueueNameEE4 =
;EE= >
_channelFF 
.FF 
	QueueBindFF 
(FF 
	queueNameFF (
,FF( )
exchangeNameFF* 6
,FF6 7 
itemUpdateRoutingKeyFF8 L
)FFL M
;FFM N
varHH 
consumerHH 
=HH 
newHH !
EventingBasicConsumerHH 4
(HH4 5
_channelHH5 =
)HH= >
;HH> ?
consumerII 
.II 
ReceivedII 
+=II  
(II! "
senderII" (
,II( )
eaII* ,
)II, -
=>II. 0
{JJ 
varKK 
bodyKK 
=KK 
eaKK 
.KK 
BodyKK "
.KK" #
ToArrayKK# *
(KK* +
)KK+ ,
;KK, -
varLL 
messageLL 
=LL 
EncodingLL &
.LL& '
UTF8LL' +
.LL+ ,
	GetStringLL, 5
(LL5 6
bodyLL6 :
)LL: ;
;LL; <
varMM 
itemMM 
=MM 
JsonSerializerMM )
.MM) *
DeserializeMM* 5
<MM5 6
ItemMM6 :
>MM: ;
(MM; <
messageMM< C
)MMC D
;MMD E
varNN 
resultNN 
=NN 
actionNN #
.NN# $
InvokeNN$ *
(NN* +
itemNN+ /
)NN/ 0
;NN0 1
ifOO 
(OO 
resultOO 
)OO 
_channelPP 
.PP 
BasicAckPP %
(PP% &
eaPP& (
.PP( )
DeliveryTagPP) 4
,PP4 5
falsePP6 ;
)PP; <
;PP< =
elseQQ 
{RR 
}TT 
}UU 
;UU
_channelVV 
.VV 
BasicConsumeVV !
(VV! "
	queueNameVV" +
,VV+ ,
falseVV- 2
,VV2 3
consumerVV4 <
)VV< =
;VV= >
}WW 	
publicYY 
voidYY 
DisposeYY 
(YY 
)YY 
{ZZ 	
if[[ 
([[ 
_channel[[ 
.[[ 
IsOpen[[ 
)[[  
{\\ 
_channel]] 
.]] 
Close]] 
(]] 
)]]  
;]]  !
_connection^^ 
.^^ 
Close^^ !
(^^! "
)^^" #
;^^# $
_channel__ 
.__ 
Dispose__  
(__  !
)__! "
;__" #
_connection`` 
.`` 
Dispose`` #
(``# $
)``$ %
;``% &
}aa 
}cc 	
}dd 
}ee �
ZC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\MessagingService\RabbitMQSettings.cs
	namespace 	
MessagingService
 
{ 
public 

class 
RabbitMQSettings !
{ 
public 
string 
HostName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
User 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
}		 