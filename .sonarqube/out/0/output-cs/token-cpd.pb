�	
cC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CatalogService.DAL\CatalogServiceDbContext.cs
	namespace 	
CatalogService
 
. 
DAL 
{ 
public 

class #
CatalogServiceDbContext (
:) *
	DbContext+ 4
{ 
public #
CatalogServiceDbContext &
(& '
DbContextOptions' 7
<7 8#
CatalogServiceDbContext8 O
>O P
optionsQ X
)X Y
:Z [
base\ `
(` a
optionsa h
)h i
{ 	
base		 
.		 
Database		 
.		 

(		' (
)		( )
;		) *
}

 	
public 
DbSet 
< 
Category 
> 

Categories )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public
DbSet
<
Item
>
Items
{
get
;
set
;
}
} 
} �
TC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CatalogService.DAL\Category.cs
	namespace 	
CatalogService
 
. 
DAL 
{ 
public 

class 
Category 
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
[		 	
Required			 
]		 
[

 	
StringLength

	 
(

 
$num

 
,

 
ErrorMessage

 &
=

' (
$str

) E
)

E F
]

F G
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Image 
{ 
get !
;! "
set# &
;& '
}( )
[

ForeignKey
(
$str
)
]
public 
int 
? 
ParentCategoryId $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
virtual 
Category 
ParentCategory  .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
} 
} �

UC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CatalogService.DAL\Configure.cs
	namespace 	
CatalogService
 
. 
DAL 
{ 
public 

static 
class 
	Configure !
{ 
public		 
static		 
IServiceCollection		 (
ConfigureDAL		) 5
(		5 6
this		6 :
IServiceCollection		; M
services		N V
)		V W
{

 	
var 
config 
= 
services !
.! " 
BuildServiceProvider" 6
(6 7
)7 8
.8 9

GetService9 C
<C D
IConfigurationD R
>R S
(S T
)T U
;U V
var 
connectionString  
=! "
config# )
.) *
GetConnectionString* =
(= >
$str> I
)I J
;J K
services
.
AddDbContext
<
CatalogServiceDbContext
>
(
opt
=>
opt
.
	UseSqlite
(
connectionString
)
)
;
return 
services 
; 
} 	
} 
} �
PC:\Users\Alan_Orozco\source\repos\NETMentoringProgram\CatalogService.DAL\Item.cs
	namespace 	
CatalogService
 
. 
DAL 
{ 
public 

class 
Item 
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
[ 	
Required	 
] 
[		 	
StringLength			 
(		 
$num		 
,		 
ErrorMessage		 &
=		' (
$str		) E
)		E F
]		F G
public

 
string

 
Name

 
{

 
get

  
;

  !
set

" %
;

% &
}

' (
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
Image 
{ 
get !
;! "
set# &
;& '
}( )
public 
virtual 
int 

CategoryId %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
public 
virtual 
Category 
Category  (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
[ 	
Required	 
] 
[ 	
Range	 
( 
$num 
, 
double 
. 
MaxValue !
)! "
]" #
public 
decimal 
Price 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
Range	 
( 
$num 
, 
double 
. 
MaxValue !
)! "
]" #
public 
int 
Amount 
{ 
get 
;  
set! $
;$ %
}& '
} 
} 