Ÿ2
mC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CartingService.WebAPI\Controllers\V1\CartsController.cs
	namespace 	
CartingService
 
. 
WebAPI 
.  
Controllers  +
.+ ,
V1, .
{ 
[		 
Route		 

(		
 
$str		  
)		  !
]		! "
[

 
ApiController

 
]

 
public 

class 
CartsController  
:! "
ControllerBase# 1
{ 
private 
readonly 
ICartingService (
_service) 1
;1 2
private 
const 
string 

apiVersion '
=( )
$str* .
;. /
public 
CartsController 
( 
ICartingService .
service/ 6
)6 7
{ 	
_service 
= 
service 
; 
} 	
[!! 	
HttpGet!!	 
(!! 
$str!! 
,!! 
Name!! !
=!!" #
nameof!!$ *
(!!* +
GetCart!!+ 2
)!!2 3
+!!4 5

apiVersion!!6 @
)!!@ A
]!!A B
["" 	 
ProducesResponseType""	 
("" 
StatusCodes"" )
."") *
Status200OK""* 5
)""5 6
]""6 7
[## 	 
ProducesResponseType##	 
(## 
StatusCodes## )
.##) *
Status404NotFound##* ;
)##; <
]##< =
[$$ 	 
ProducesResponseType$$	 
($$ 
StatusCodes$$ )
.$$) *(
Status500InternalServerError$$* F
)$$F G
]$$G H
public%% 
async%% 
Task%% 
<%% 
ActionResult%% &
<%%& '
Cart%%' +
>%%+ ,
>%%, -
GetCart%%. 5
(%%5 6
[%%6 7
	FromRoute%%7 @
]%%@ A
Guid%%B F
cartId%%G M
)%%M N
{&& 	
var'' 
cart'' 
='' 
await'' 
_service'' %
.''% &
GetCart''& -
(''- .
cartId''. 4
)''4 5
;''5 6
if(( 
((( 
cart(( 
==(( 
null(( 
)(( 
return)) 
NotFound)) 
())  
)))  !
;))! "
return** 
Ok** 
(** 
cart** 
)** 
;** 
}++ 	
[;; 	
HttpPost;;	 
(;; 
$str;; 
,;; 
Name;; "
=;;# $
nameof;;% +
(;;+ ,
AddItemtoCart;;, 9
);;9 :
+;;; <

apiVersion;;= G
);;G H
];;H I
[<< 	 
ProducesResponseType<<	 
(<< 
StatusCodes<< )
.<<) *
Status200OK<<* 5
)<<5 6
]<<6 7
[== 	 
ProducesResponseType==	 
(== 
StatusCodes== )
.==) *
Status400BadRequest==* =
)=== >
]==> ?
[>> 	 
ProducesResponseType>>	 
(>> 
StatusCodes>> )
.>>) *
Status404NotFound>>* ;
)>>; <
]>>< =
[?? 	 
ProducesResponseType??	 
(?? 
StatusCodes?? )
.??) *(
Status500InternalServerError??* F
)??F G
]??G H
public@@ 
async@@ 
Task@@ 
<@@ 
ActionResult@@ &
>@@& '
AddItemtoCart@@( 5
(@@5 6
Guid@@6 :
cartId@@; A
,@@A B
[@@C D
FromBody@@D L
]@@L M
Item@@N R
item@@S W
)@@W X
{AA 	
varBB 

existsCartBB 
=BB 
awaitBB "
_serviceBB# +
.BB+ ,

ExistsCartBB, 6
(BB6 7
cartIdBB7 =
)BB= >
;BB> ?
ifCC 
(CC 

existsCartCC 
)CC 
awaitDD 
_serviceDD 
.DD 
AddItemDD &
(DD& '
cartIdDD' -
,DD- .
itemDD/ 3
)DD3 4
;DD4 5
elseEE 
awaitFF 
_serviceFF 
.FF 
InitializeCartFF -
(FF- .
cartIdFF. 4
,FF4 5
itemFF6 :
)FF: ;
;FF; <
returnGG 
OkGG 
(GG 
)GG 
;GG 
}HH 	
[VV 	

HttpDeleteVV	 
(VV 
$strVV '
,VV' (
NameVV) -
=VV. /
nameofVV0 6
(VV6 7
RemoveItemFromCartVV7 I
)VVI J
+VVK L

apiVersionVVM W
)VVW X
]VVX Y
[WW 	 
ProducesResponseTypeWW	 
(WW 
StatusCodesWW )
.WW) *
Status200OKWW* 5
)WW5 6
]WW6 7
[XX 	 
ProducesResponseTypeXX	 
(XX 
StatusCodesXX )
.XX) *
Status204NoContentXX* <
)XX< =
]XX= >
[YY 	 
ProducesResponseTypeYY	 
(YY 
StatusCodesYY )
.YY) *(
Status500InternalServerErrorYY* F
)YYF G
]YYG H
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 
ActionResultZZ &
>ZZ& '
RemoveItemFromCartZZ( :
(ZZ: ;
GuidZZ; ?
cartIdZZ@ F
,ZZF G
intZZH K
itemIdZZL R
)ZZR S
{[[ 	
var\\ 

existsCart\\ 
=\\ 
await\\ "
_service\\# +
.\\+ ,
ExistsItemOnCart\\, <
(\\< =
cartId\\= C
,\\C D
itemId\\E K
)\\K L
;\\L M
if]] 
(]] 

existsCart]] 
)]] 
{^^ 
await__ 
_service__ 
.__ 

RemoveItem__ )
(__) *
cartId__* 0
,__0 1
itemId__2 8
)__8 9
;__9 :
return`` 
Ok`` 
(`` 
)`` 
;`` 
}aa 
elsebb 
{cc 
returndd 
	NoContentdd  
(dd  !
)dd! "
;dd" #
}ee 
}ff 	
}gg 
}hh û2
mC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CartingService.WebAPI\Controllers\V2\CartsController.cs
	namespace 	
CartingService
 
. 
WebAPI 
.  
Controllers  +
.+ ,
V2, .
{ 
[		 
Route		 

(		
 
$str		  
)		  !
]		! "
[

 
ApiController

 
]

 
public 

class 
CartsController  
:! "
ControllerBase# 1
{ 
private 
readonly 
ICartingService (
_service) 1
;1 2
private 
const 
string 

apiVersion '
=( )
$str* .
;. /
public 
CartsController 
( 
ICartingService .
service/ 6
)6 7
{ 	
_service 
= 
service 
; 
} 	
[!! 	
HttpGet!!	 
(!! 
$str!! 
,!! 
Name!! !
=!!" #
nameof!!$ *
(!!* +
GetCartItems!!+ 7
)!!7 8
+!!9 :

apiVersion!!; E
)!!E F
]!!F G
["" 	 
ProducesResponseType""	 
("" 
StatusCodes"" )
."") *
Status200OK""* 5
)""5 6
]""6 7
[## 	 
ProducesResponseType##	 
(## 
StatusCodes## )
.##) *
Status404NotFound##* ;
)##; <
]##< =
[$$ 	 
ProducesResponseType$$	 
($$ 
StatusCodes$$ )
.$$) *(
Status500InternalServerError$$* F
)$$F G
]$$G H
public%% 
async%% 
Task%% 
<%% 
ActionResult%% &
<%%& '
List%%' +
<%%+ ,
Item%%, 0
>%%0 1
>%%1 2
>%%2 3
GetCartItems%%4 @
(%%@ A
[%%A B
	FromRoute%%B K
]%%K L
Guid%%M Q
cartId%%R X
)%%X Y
{&& 	
var'' 
cart'' 
='' 
await'' 
_service'' %
.''% &
GetCart''& -
(''- .
cartId''. 4
)''4 5
;''5 6
if(( 
((( 
cart(( 
==(( 
null(( 
)(( 
return)) 
NotFound)) 
())  
)))  !
;))! "
return** 
Ok** 
(** 
cart** 
.** 
Items**  
)**  !
;**! "
}++ 	
[;; 	
HttpPost;;	 
(;; 
$str;; 
,;; 
Name;; "
=;;# $
nameof;;% +
(;;+ ,
AddItemtoCart;;, 9
);;9 :
+;;; <

apiVersion;;= G
);;G H
];;H I
[<< 	 
ProducesResponseType<<	 
(<< 
StatusCodes<< )
.<<) *
Status200OK<<* 5
)<<5 6
]<<6 7
[== 	 
ProducesResponseType==	 
(== 
StatusCodes== )
.==) *
Status400BadRequest==* =
)=== >
]==> ?
[>> 	 
ProducesResponseType>>	 
(>> 
StatusCodes>> )
.>>) *
Status404NotFound>>* ;
)>>; <
]>>< =
[?? 	 
ProducesResponseType??	 
(?? 
StatusCodes?? )
.??) *(
Status500InternalServerError??* F
)??F G
]??G H
public@@ 
async@@ 
Task@@ 
<@@ 
ActionResult@@ &
>@@& '
AddItemtoCart@@( 5
(@@5 6
Guid@@6 :
cartId@@; A
,@@A B
[@@C D
FromBody@@D L
]@@L M
Item@@N R
item@@S W
)@@W X
{AA 	
varBB 

existsCartBB 
=BB 
awaitBB "
_serviceBB# +
.BB+ ,

ExistsCartBB, 6
(BB6 7
cartIdBB7 =
)BB= >
;BB> ?
ifCC 
(CC 

existsCartCC 
)CC 
awaitDD 
_serviceDD 
.DD 
AddItemDD &
(DD& '
cartIdDD' -
,DD- .
itemDD/ 3
)DD3 4
;DD4 5
elseEE 
awaitFF 
_serviceFF 
.FF 
InitializeCartFF -
(FF- .
cartIdFF. 4
,FF4 5
itemFF6 :
)FF: ;
;FF; <
returnGG 
OkGG 
(GG 
)GG 
;GG 
}HH 	
[VV 	

HttpDeleteVV	 
(VV 
$strVV '
,VV' (
NameVV) -
=VV. /
nameofVV0 6
(VV6 7
RemoveItemFromCartVV7 I
)VVI J
+VVK L

apiVersionVVM W
)VVW X
]VVX Y
[WW 	 
ProducesResponseTypeWW	 
(WW 
StatusCodesWW )
.WW) *
Status200OKWW* 5
)WW5 6
]WW6 7
[XX 	 
ProducesResponseTypeXX	 
(XX 
StatusCodesXX )
.XX) *
Status204NoContentXX* <
)XX< =
]XX= >
[YY 	 
ProducesResponseTypeYY	 
(YY 
StatusCodesYY )
.YY) *(
Status500InternalServerErrorYY* F
)YYF G
]YYG H
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 
ActionResultZZ &
>ZZ& '
RemoveItemFromCartZZ( :
(ZZ: ;
GuidZZ; ?
cartIdZZ@ F
,ZZF G
intZZH K
itemIdZZL R
)ZZR S
{[[ 	
var\\ 

existsCart\\ 
=\\ 
await\\ "
_service\\# +
.\\+ ,
ExistsItemOnCart\\, <
(\\< =
cartId\\= C
,\\C D
itemId\\E K
)\\K L
;\\L M
if]] 
(]] 

existsCart]] 
)]] 
{^^ 
await__ 
_service__ 
.__ 

RemoveItem__ )
(__) *
cartId__* 0
,__0 1
itemId__2 8
)__8 9
;__9 :
return`` 
Ok`` 
(`` 
)`` 
;`` 
}aa 
elsebb 
{cc 
returndd 
	NoContentdd  
(dd  !
)dd! "
;dd" #
}ee 
}ff 	
}gg 
}hh š 
VC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CartingService.WebAPI\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
options  '
=>( *
{ 
options 
. 
Conventions 
. 
Add 
( 
new !
SwaggerGroupByVersion  5
(5 6
)6 7
)7 8
;8 9
} 
) 
; 
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
options &
=>' )
{ 
options 
. 

SwaggerDoc 
( 
$str 
, 
new  
OpenApiInfo! ,
(, -
)- .
{ 
Title 
= 
$str $
,$ %
Version 
= 
$str 
, 
Description 
= 
$str <
} 
) 
; 
options 
. 

SwaggerDoc 
( 
$str 
, 
new  
OpenApiInfo! ,
(, -
)- .
{ 
Title 
= 
$str $
,$ %
Version 
= 
$str 
, 
Description 
= 
$str <
} 
) 
; 
var 
xmlFilename 
= 
$" 
{ 
Assembly !
.! " 
GetExecutingAssembly" 6
(6 7
)7 8
.8 9
GetName9 @
(@ A
)A B
.B C
NameC G
}G H
$strH L
"L M
;M N
options   
.   
IncludeXmlComments   
(   
Path   #
.  # $
Combine  $ +
(  + ,

AppContext  , 6
.  6 7
BaseDirectory  7 D
,  D E
xmlFilename  F Q
)  Q R
)  R S
;  S T
}!! 
)!! 
;!! 
builder## 
.## 
Services## 
.## 
ConfigureDAL## 
(## 
)## 
.$$ 
ConfigureBLL$$ 
($$ 
)$$ 
;$$  
var&& 
app&& 
=&& 	
builder&&
 
.&& 
Build&& 
(&& 
)&& 
;&& 
if)) 
()) 
app)) 
.)) 
Environment)) 
.)) 
IsDevelopment)) !
())! "
)))" #
)))# $
{** 
app++ 
.++ 

UseSwagger++ 
(++ 
)++ 
;++ 
app,, 
.,, 
UseSwaggerUI,, 
(,, 
c,, 
=>,, 
{-- 
c.. 	
...	 

SwaggerEndpoint..
 
(.. 
$str.. 4
,..4 5
$str..6 I
)..I J
;..J K
c// 	
.//	 

SwaggerEndpoint//
 
(// 
$str// 4
,//4 5
$str//6 I
)//I J
;//J K
}00 
)00 
;00 
}11 
app33 
.33 
UseHttpsRedirection33 
(33 
)33 
;33 
app55 
.55 
UseAuthorization55 
(55 
)55 
;55 
app77 
.77 
MapControllers77 
(77 
)77 
;77 
app99 
.99 
Run99 
(99 
)99 	
;99	 
®	
dC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CartingService.WebAPI\SwaggerGroupByVersion.cs
	namespace 	
CartingService
 
. 
WebAPI 
{ 
public 

class !
SwaggerGroupByVersion &
:' (&
IControllerModelConvention) C
{ 
public 
void 
Apply 
( 
ControllerModel )

controller* 4
)4 5
{ 	
var		 
namespaceController		 #
=		$ %

controller		& 0
.		0 1
ControllerType		1 ?
.		? @
	Namespace		@ I
;		I J
var

 

versionAPI

 
=

 
namespaceController

 0
.

0 1
Split

1 6
(

6 7
$char

7 :
)

: ;
.

; <
Last

< @
(

@ A
)

A B
.

B C
ToLower

C J
(

J K
)

K L
;

L M

controller 
. 
ApiExplorer "
." #
	GroupName# ,
=- .

versionAPI/ 9
;9 :
} 	
} 
} 