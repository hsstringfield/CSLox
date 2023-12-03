all : clean restore build Test1 Test2 Test3 Test4 Test5 Test6

clean:
	dotnet clean

restore:
	dotnet restore

build: 
	dotnet build

Test1:
	dotnet run Test1.txt

Test2:
	dotnet run Test2.txt

Test3:
	dotnet run Test3.txt

Test4:
	dotnet run Test4.txt

Test5:
	dotnet run Test5.txt

Test6:
	dotnet run Test6.txt
