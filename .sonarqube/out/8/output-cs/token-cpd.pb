Š0
oC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CatalogService.WebAPI\Controllers\CategoriesController.cs
	namespace 	
CatalogService
 
. 
WebAPI 
.  
Controllers  +
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
public		 

class		  
CategoriesController		 %
:		& '
ControllerBase		( 6
{

 
private 
readonly 
ICatalogService (
_service) 1
;1 2
public  
CategoriesController #
(# $
ICatalogService$ 3
service4 ;
); <
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
( 
Name 
= 
nameof 
( 
GetCategories ,
), -
)- .
]. /
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *
Status200OK* 5
)5 6
]6 7
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *(
Status500InternalServerError* F
)F G
]G H
public 
async 
Task 
< 
ActionResult &
<& '
IEnumerable' 2
<2 3
Category3 ;
>; <
>< =
>= >
GetCategories? L
(L M
)M N
{ 	
var 

categories 
= 
await "
_service# +
.+ ,
GetAllCategories, <
(< =
)= >
;> ?
return 
Ok 
( 

categories  
)  !
;! "
} 	
[ 	
HttpPost	 
( 
Name 
= 
nameof 
(  
AddCategory  +
)+ ,
), -
]- .
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *
Status201Created* :
): ;
]; <
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *(
Status500InternalServerError* F
)F G
]G H
public 
async 
Task 
< 
ActionResult &
<& '
Category' /
>/ 0
>0 1
AddCategory2 =
(= >
[> ?
FromBody? G
]G H
CategoryDTOI T
categoryU ]
)] ^
{ 	
var   
createdCategory   
=    !
await  " '
_service  ( 0
.  0 1
AddCategory  1 <
(  < =
category  = E
)  E F
;  F G
return!! 
CreatedAtAction!! "
(!!" #
nameof!!# )
(!!) *
AddCategory!!* 5
)!!5 6
,!!6 7
new!!8 ;
{!!< =
id!!> @
=!!A B
createdCategory!!C R
.!!R S
Id!!S U
}!!V W
,!!W X
createdCategory!!Y h
)!!h i
;!!i j
}"" 	
[## 	
HttpPut##	 
(## 
$str## 
,## 
Name## 
=## 
nameof##  &
(##& '
UpdateCategory##' 5
)##5 6
)##6 7
]##7 8
[$$ 	 
ProducesResponseType$$	 
($$ 
StatusCodes$$ )
.$$) *
Status204NoContent$$* <
)$$< =
]$$= >
[%% 	 
ProducesResponseType%%	 
(%% 
StatusCodes%% )
.%%) *
Status400BadRequest%%* =
)%%= >
]%%> ?
[&& 	 
ProducesResponseType&&	 
(&& 
StatusCodes&& )
.&&) *
Status404NotFound&&* ;
)&&; <
]&&< =
['' 	 
ProducesResponseType''	 
('' 
StatusCodes'' )
.'') *(
Status500InternalServerError''* F
)''F G
]''G H
public(( 
async(( 
Task(( 
<(( 
ActionResult(( &
>((& '
UpdateCategory((( 6
(((6 7
[((7 8
	FromRoute((8 A
]((A B
int((C F
id((G I
,((I J
[((K L
FromBody((L T
]((T U
CategoryDTO((V a
categoryDTO((b m
)((m n
{)) 	
await** 
_service** 
.** 
UpdateCategory** )
(**) *
id*** ,
,**, -
categoryDTO**. 9
)**9 :
;**: ;
return++ 
	NoContent++ 
(++ 
)++ 
;++ 
},, 	
[-- 	

HttpDelete--	 
(-- 
$str-- 
,-- 
Name--  
=--! "
nameof--# )
(--) *
DeleteCategory--* 8
)--8 9
)--9 :
]--: ;
[.. 	 
ProducesResponseType..	 
(.. 
StatusCodes.. )
...) *
Status204NoContent..* <
)..< =
]..= >
[// 	 
ProducesResponseType//	 
(// 
StatusCodes// )
.//) *(
Status500InternalServerError//* F
)//F G
]//G H
public00 
async00 
Task00 
<00 
ActionResult00 &
>00& '
DeleteCategory00( 6
(006 7
[007 8
	FromRoute008 A
]00A B
int00C F
id00G I
)00I J
{11 	
await22 
_service22 
.22 
DeleteCategory22 )
(22) *
id22* ,
)22, -
;22- .
return33 
	NoContent33 
(33 
)33 
;33 
}44 	
}66 
}77 –U
jC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CatalogService.WebAPI\Controllers\ItemsController.cs
	namespace 	
CatalogService
 
. 
WebAPI 
.  
Controllers  +
{ 
[ 
Route 

(
 
$str 
) 
] 
[		 
ApiController		 
]		 
public

 

class

 
ItemsController

  
:

! "
ControllerBase

# 1
{ 
private 
readonly 
ICatalogService (
_service) 1
;1 2
public 
ItemsController 
( 
ICatalogService .
service/ 6
)6 7
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
( 
Name 
= 
nameof 
( 
GetItems '
)' (
)( )
]) *
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *
Status200OK* 5
)5 6
]6 7
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *(
Status500InternalServerError* F
)F G
]G H
public 
async 
Task 
< 
ActionResult &
<& '
IEnumerable' 2
<2 3
Item3 7
>7 8
>8 9
>9 :
GetItems; C
(C D
[D E
	FromQueryE N
]N O
	ItemQueryP Y
	itemQueryZ c
)c d
{ 	
var 
items 
= 
await 
_service &
.& '
GetItems' /
(/ 0
	itemQuery0 9
)9 :
;: ;
Response 
. 
Headers 
. 
Add  
(  !
$str! /
,/ 0
items1 6
.6 7
ToPaginationHeader7 I
(I J
)J K
)K L
;L M
return 
Ok 
( 
items 
) 
; 
} 	
[ 	
HttpGet	 
( 
$str 
, 
Name 
= 
nameof  &
(& '
GetItem' .
). /
)/ 0
]0 1
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *
Status200OK* 5
)5 6
]6 7
[ 	 
ProducesResponseType	 
( 
StatusCodes )
.) *
Status404NotFound* ;
); <
]< =
[   	 
ProducesResponseType  	 
(   
StatusCodes   )
.  ) *(
Status500InternalServerError  * F
)  F G
]  G H
public!! 
async!! 
Task!! 
<!! 
ActionResult!! &
<!!& '
Item!!' +
>!!+ ,
>!!, -
GetItem!!. 5
(!!5 6
[!!6 7
	FromRoute!!7 @
]!!@ A
int!!B E
id!!F H
)!!H I
{"" 	
var## 
item## 
=## 
await## 
_service## %
.##% &
GetItem##& -
(##- .
id##. 0
)##0 1
;##1 2
if$$ 
($$ 
item$$ 
==$$ 
null$$ 
)$$ 
return%% 
NotFound%% 
(%%  
)%%  !
;%%! "
return&& 
Ok&& 
(&& 
item&& 
)&& 
;&& 
}'' 	
[)) 	
HttpGet))	 
()) 
$str)) 
,))  
Name))! %
=))& '
nameof))( .
()). /
GetItemDetails))/ =
)))= >
)))> ?
]))? @
[** 	 
ProducesResponseType**	 
(** 
StatusCodes** )
.**) *
Status200OK*** 5
)**5 6
]**6 7
[++ 	 
ProducesResponseType++	 
(++ 
StatusCodes++ )
.++) *
Status404NotFound++* ;
)++; <
]++< =
[,, 	 
ProducesResponseType,,	 
(,, 
StatusCodes,, )
.,,) *(
Status500InternalServerError,,* F
),,F G
],,G H
public-- 
async-- 
Task-- 
<-- 
ActionResult-- &
<--& '
ItemDetails--' 2
>--2 3
>--3 4
GetItemDetails--5 C
(--C D
[--D E
	FromRoute--E N
]--N O
int--P S
id--T V
)--V W
{.. 	
var// 
itemDetails// 
=// 
await// #
_service//$ ,
.//, -
GetItemDetails//- ;
(//; <
id//< >
)//> ?
;//? @
if00 
(00 
itemDetails00 
==00 
null00 #
)00# $
return11 
NotFound11 
(11  
)11  !
;11! "
return22 
Ok22 
(22 
itemDetails22 !
)22! "
;22" #
}33 	
[55 	
	Authorize55	 
(55 
Roles55 
=55 
$str55 ,
)55, -
]55- .
[66 	
HttpPost66	 
(66 
Name66 
=66 
nameof66 
(66  
AddItem66  '
)66' (
)66( )
]66) *
[77 	 
ProducesResponseType77	 
(77 
StatusCodes77 )
.77) *
Status201Created77* :
)77: ;
]77; <
[88 	 
ProducesResponseType88	 
(88 
StatusCodes88 )
.88) *!
Status401Unauthorized88* ?
)88? @
]88@ A
[99 	 
ProducesResponseType99	 
(99 
StatusCodes99 )
.99) *
Status403Forbidden99* <
)99< =
]99= >
[:: 	 
ProducesResponseType::	 
(:: 
StatusCodes:: )
.::) *(
Status500InternalServerError::* F
)::F G
]::G H
public;; 
async;; 
Task;; 
<;; 
ActionResult;; &
<;;& '
Item;;' +
>;;+ ,
>;;, -
AddItem;;. 5
(;;5 6
[;;6 7
FromBody;;7 ?
];;? @
ItemDTO;;A H
item;;I M
);;M N
{<< 	
var== 
createdItem== 
=== 
await== #
_service==$ ,
.==, -
AddItem==- 4
(==4 5
item==5 9
)==9 :
;==: ;
return>> 
CreatedAtAction>> "
(>>" #
nameof>># )
(>>) *
AddItem>>* 1
)>>1 2
,>>2 3
new>>4 7
{>>8 9
id>>: <
=>>= >
createdItem>>? J
.>>J K
Id>>K M
}>>N O
,>>O P
createdItem>>Q \
)>>\ ]
;>>] ^
}?? 	
[AA 	
	AuthorizeAA	 
(AA 
RolesAA 
=AA 
$strAA ,
)AA, -
]AA- .
[BB 	
HttpPutBB	 
(BB 
$strBB 
,BB 
NameBB 
=BB 
nameofBB  &
(BB& '

UpdateItemBB' 1
)BB1 2
)BB2 3
]BB3 4
[CC 	 
ProducesResponseTypeCC	 
(CC 
StatusCodesCC )
.CC) *
Status204NoContentCC* <
)CC< =
]CC= >
[DD 	 
ProducesResponseTypeDD	 
(DD 
StatusCodesDD )
.DD) *
Status400BadRequestDD* =
)DD= >
]DD> ?
[EE 	 
ProducesResponseTypeEE	 
(EE 
StatusCodesEE )
.EE) *!
Status401UnauthorizedEE* ?
)EE? @
]EE@ A
[FF 	 
ProducesResponseTypeFF	 
(FF 
StatusCodesFF )
.FF) *
Status403ForbiddenFF* <
)FF< =
]FF= >
[GG 	 
ProducesResponseTypeGG	 
(GG 
StatusCodesGG )
.GG) *
Status404NotFoundGG* ;
)GG; <
]GG< =
[HH 	 
ProducesResponseTypeHH	 
(HH 
StatusCodesHH )
.HH) *(
Status500InternalServerErrorHH* F
)HHF G
]HHG H
publicII 
asyncII 
TaskII 
<II 
ActionResultII &
>II& '

UpdateItemII( 2
(II2 3
[II3 4
	FromRouteII4 =
]II= >
intII? B
idIIC E
,IIE F
[IIG H
FromBodyIIH P
]IIP Q
ItemDTOIIR Y
itemIIZ ^
)II^ _
{JJ 	
awaitKK 
_serviceKK 
.KK 

UpdateItemKK %
(KK% &
idKK& (
,KK( )
itemKK* .
)KK. /
;KK/ 0
returnLL 
	NoContentLL 
(LL 
)LL 
;LL 
}MM 	
[OO 	
	AuthorizeOO	 
(OO 
RolesOO 
=OO 
$strOO ,
)OO, -
]OO- .
[PP 	

HttpDeletePP	 
(PP 
$strPP 
,PP 
NamePP  
=PP! "
nameofPP# )
(PP) *

DeleteItemPP* 4
)PP4 5
)PP5 6
]PP6 7
[QQ 	 
ProducesResponseTypeQQ	 
(QQ 
StatusCodesQQ )
.QQ) *
Status204NoContentQQ* <
)QQ< =
]QQ= >
[RR 	 
ProducesResponseTypeRR	 
(RR 
StatusCodesRR )
.RR) *!
Status401UnauthorizedRR* ?
)RR? @
]RR@ A
[SS 	 
ProducesResponseTypeSS	 
(SS 
StatusCodesSS )
.SS) *
Status403ForbiddenSS* <
)SS< =
]SS= >
[TT 	 
ProducesResponseTypeTT	 
(TT 
StatusCodesTT )
.TT) *(
Status500InternalServerErrorTT* F
)TTF G
]TTG H
publicUU 
asyncUU 
TaskUU 
<UU 
ActionResultUU &
>UU& '

DeleteItemUU( 2
(UU2 3
[UU3 4
	FromRouteUU4 =
]UU= >
intUU? B
idUUC E
)UUE F
{VV 	
awaitWW 
_serviceWW 
.WW 

DeleteItemWW %
(WW% &
idWW& (
)WW( )
;WW) *
returnXX 
	NoContentXX 
(XX 
)XX 
;XX 
}YY 	
}ZZ 
}[[ Ÿ*
VC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CatalogService.WebAPI\Program.cs
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
builder

 
.

 
Services

 
.

 
ConfigureDAL

 
(

 
)

 
. 
ConfigureBLL 
( 
) 
; 
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
)  
;  !
builder 
. 
Services 
. 4
(AddMicrosoftIdentityWebApiAuthentication 9
(9 :
builder: A
.A B
ConfigurationB O
)O P
. 5
)EnableTokenAcquisitionToCallDownstreamApi :
(: ;
opt; >
=>? A
optB E
.E F
EnablePiiLoggingF V
=W X
falseY ^
)^ _
. 
AddMicrosoftGraph "
(" #
builder# *
.* +
Configuration+ 8
.8 9

GetSection9 C
(C D
$strD S
)S T
)T U
. "
AddInMemoryTokenCaches '
(' (
)( )
;) *
var 
app 
= 	
builder
 
. 
Build 
( 
) 
; 
if 
( 
app 
. 
Environment 
. 
IsDevelopment !
(! "
)" #
)# $
{ 
app 
. 

UseSwagger 
( 
) 
; 
app 
. 
UseSwaggerUI 
( 
) 
; 
} 
app 
. 
UseExceptionHandler 
( 
excHApp 
=>  "
{ 
excHApp 
. 
Run 
( 
async 
context 
=>  
{   
context!! 
.!! 
Response!! 
.!! 

StatusCode!! #
=!!$ %
StatusCodes!!& 1
.!!1 2(
Status500InternalServerError!!2 N
;!!N O
context"" 
."" 
Response"" 
."" 
ContentType"" $
=""% &
Text""' +
.""+ ,
Plain"", 1
;""1 2
var## 
	exception## 
=## 
context## 
.##  
Features##  (
.##( )
Get##) ,
<##, -(
IExceptionHandlerPathFeature##- I
>##I J
(##J K
)##K L
;##L M
if$$ 

($$ 
	exception$$ 
?$$ 
.$$ 
Error$$ 
is$$  
KeyNotFoundException$$  4
)$$4 5
{%% 	
context&& 
.&& 
Response&& 
.&& 

StatusCode&& '
=&&( )
StatusCodes&&* 5
.&&5 6
Status404NotFound&&6 G
;&&G H
await'' 
context'' 
.'' 
Response'' "
.''" #

WriteAsync''# -
(''- .
	exception''. 7
.''7 8
Error''8 =
.''= >
Message''> E
)''E F
;''F G
}(( 	
else)) 
if)) 
()) 
	exception)) 
?)) 
.)) 
Error)) !
is))" $
ArgumentException))% 6
)))6 7
{** 	
context++ 
.++ 
Response++ 
.++ 

StatusCode++ '
=++( )
StatusCodes++* 5
.++5 6
Status400BadRequest++6 I
;++I J
await,, 
context,, 
.,, 
Response,, "
.,," #

WriteAsync,,# -
(,,- .
	exception,,. 7
.,,7 8
Error,,8 =
.,,= >
Message,,> E
),,E F
;,,F G
}-- 	
else.. 
{// 	
await00 
context00 
.00 
Response00 "
.00" #

WriteAsync00# -
(00- .
$str00. D
)00D E
;00E F
}11 	
}22 
)22 
;22 
}33 
)33 
;33 
app55 
.55 
UseHttpsRedirection55 
(55 
)55 
;55 
app77 
.77 
UseAuthentication77 
(77 
)77 
;77 
app88 
.88 
UseAuthorization88 
(88 
)88 
;88 
app:: 
.:: 

MapGraphQL:: 
(:: 
):: 
;:: 
app<< 
.<< 
MapControllers<< 
(<< 
)<< 
;<< 
app>> 
.>> 
Run>> 
(>> 
)>> 	
;>>	 
