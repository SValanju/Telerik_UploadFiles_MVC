use DcodeTech;

create table SaveFiles(
ID int identity primary key,
vId int,
FileName varchar(300),
FileDesc varchar(300),
FilePath varchar(300)
);

select * from SaveFiles;
