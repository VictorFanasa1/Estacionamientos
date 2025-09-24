Use Estacionamiento

--------------------------------------------------------------------------------------------
------------------------------------------ Tabla de Rol ------------------------
Create table roles
(
	uiRegistroRol int Identity(1,1) not null,
	sRol varchar(50)
);

insert into roles (sRol) values('Empleado')
insert into roles (sRol) values('Visitante')
insert into roles (sRol) values('Empleado Solicitante')

Select * from roles
------------------------------------------ Tabla de Usuarios -----------------------------
Create table usuarios
(
	IdUsuarios int Identity(1,1) not null,
	uiNumeroEmpleado int,
	sNombre varchar(200),
	sCorreo varchar(100),
	iRol int,
	bActivo bit
	Primary Key(IdUsuarios)
);

Select * from usuarios
delete from usuarios

DBCC CHECKIDENT (usuarios, RESEED, 0)

alter table usuarios add idUserResponsables int

--------------------------------------------------------------------------------------------------------------------
-------------------------------------------- Catálogo de Lugares ------------------------------------------------------

Create table catLugares
(
	uiRegistroCatalogo int Identity(1,1) not null,
	sLugar varchar(200),
	Primary Key(uiRegistroCatalogo)
);

--insert into catLugares (sLugar) values ('Balmis 178')
--insert into catLugares (sLugar) values ('Pasteur 93')
--insert into catLugares (sLugar) values ('Balmis 180')
--insert into catLugares (sLugar) values ('Balmis 181')
--insert into catLugares (sLugar) values ('Balmis 182')
--insert into catLugares (sLugar) values ('Lucio 254')
--insert into catLugares (sLugar) values ('Dr. Olvera 172')
--insert into catLugares (sLugar) values ('Queretaro 133')
--insert into catLugares (sLugar) values ('Queretaro 171')

Select * from catLugares
delete from catLugares where uiRegistroCatalogo = 10

----------------------------------------------------------------------------
------------------------------------------------- Table Catalogo Lugares ------------------------------------

Create table catNumeroLugares
(
	uiRegistroLugar int Identity(1,1) not null,
	sLugar varchar(50),
	iLugar int
	Primary Key(uiRegistroLugar)
);

Select * from catNumeroLugares where iLugar = 1 and bActivo = 1
update catNumeroLugares set iLugar = 5 where uiRegistroLugar = 106

Select * from catNumeroLugares where iLugar = 8 and sLugar = '84'


--alter table catNumeroLugares add bActivo bit
--update catNumeroLugares set bActivo = 1




--------------------------------------------------------------------------------------------------------------------
-------------------------------------------- Catálogo de Lugares ------------------------------------------------------

Create table cajonesEstacionamientos
(
	uiRegistroCajones int Identity(1,1) not null,
	iCajon int,
	iLugar int,
	iUsuario int,
	sObservaciones nvarchar(max),
	dtFechaAsignacion Date
	Primary Key(uiRegistroCajones)
);

--alter table cajonesEstacionamientos add dtFechaAsignacion Date
Select * from cajonesEstacionamientos where uiRegistroCajones = 12

--alter table cajonesEstacionamientos add sPlaca varchar(50)
--alter table cajonesEstacionamientos add sModelo varchar(50)
--alter table cajonesEstacionamientos add sColor varchar(50)

Select * from usuarios 

delete from usuarios
delete from cajonesEstacionamientos
DBCC CHECKIDENT (cajonesEstacionamientos, RESEED, 0)


--alter table cajonesEstacionamientos add dtFechaVisita Date
--alter table cajonesEstacionamientos add horaVisita varchar(50)
--alter table cajonesEstacionamientos add horaSalida varchar(50)
--alter table cajonesEstacionamientos add totalHorasVisita int

--alter table cajonesEstacionamientos drop column totalHorasVisita

--alter table cajonesEstacionamientos add bActivo Bit

------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------


------------------------------------ SP INSERT Usuarios ------------------------

Alter proc sp_InsertUsuariosEmpleados
@uiNumeroEmpleado int,
@sNombre varchar(200),
@sCorreo varchar(100),
@IdUsuarios int output
AS
BEGIN
	BEGIN TRY
	Insert into usuarios (uiNumeroEmpleado, sNombre, sCorreo, iRol, bActivo)
	Values(@uiNumeroEmpleado, @sNombre, @sCorreo, 1,1)
	Select @IdUsuarios = SCOPE_IDENTITY()
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

-----------------------------------------------------------------------------------------------

------------------------------------ SP Cambiar Estatus Lugar ------------------------

Create proc sp_CambiarStatusLugar
@iLugar int,
@sLugar varchar(50)
AS
BEGIN
	BEGIN TRY
	update catNumeroLugares set bActivo = 0 where iLugar = @iLugar and sLugar = @sLugar
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

Select * from catNumeroLugares where iLugar = 3 and sLugar = '84'

Select * from catNumeroLugares where bActivo = 1

-----------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------
------------------------------------ SP INSERT Visitantes -------------------------------------

Alter proc sp_InsertUsuariosVisitantes
@iRegistroUserResponsable int,
@sNombre varchar(200),
@IdUsuarios int output
AS
BEGIN
	BEGIN TRY
	Insert into usuarios (sNombre,  iRol, idUserResponsables, bActivo)
	Values(@sNombre, 2, @iRegistroUserResponsable,1)
	Select @IdUsuarios = SCOPE_IDENTITY()
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

-----------------------------------------------------------------------------------------------
------------------------------------ SP INSERT Usuarios Responsables ------------------------

Create proc sp_InsertUsuariosEmpleadosResponsables
@uiNumeroEmpleado int,
@sNombre varchar(200),
@sCorreo varchar(100),
@IdUsuarios int output
AS
BEGIN
	BEGIN TRY
	Insert into usuarios (uiNumeroEmpleado, sNombre, sCorreo, iRol)
	Values(@uiNumeroEmpleado, @sNombre, @sCorreo, 3)
	Select @IdUsuarios = SCOPE_IDENTITY()
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

-----------------------------------------------------------------------------------------------

------------------------------------ SP INSERT Estacionamiento ------------------------

Alter proc sp_InsertEstacionamiento
@iCajon int,
@iLugar int,
@iUsuario int,
@sObservaciones nvarchar(max) = null
AS
BEGIN
	BEGIN TRY
	INSERT INTO cajonesEstacionamientos (iCajon, iLugar, iUsuario, sObservaciones, dtFechaAsignacion, bActivo)
	values(@iCajon, @iLugar, @iUsuario, @sObservaciones,  GETDATE(),1)
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END


-----------------------------------------------------------------------------------------------

------------------------------------ SP INSERT Estacionamiento Visitantes------------------------

Alter proc sp_InsertEstacionamientoVisitantes
@iCajon int = null,
@iLugar int = null,
@iUsuario int,
@sObservaciones nvarchar(max) = null,
@dtFechaVisita Date,
@horaVisita varchar(50),
@horaSalida varchar(50),
@sPlaca varchar(50),
@sModelo varchar(50),
@sColor varchar(50),
@uiRegistroCajones int output
AS
BEGIN
	BEGIN TRY
	INSERT INTO cajonesEstacionamientos (iCajon, iLugar, iUsuario, sObservaciones, dtFechaAsignacion, dtFechaVisita, horaVisita, horaSalida, bActivo, sPlaca, sModelo, sColor)
	values(@iCajon, @iLugar, @iUsuario, @sObservaciones, GETDATE(), @dtFechaVisita, @horaVisita, @horaSalida,1, @sPlaca, @sModelo, @sColor)
	Select @uiRegistroCajones = SCOPE_IDENTITY()
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END


-----------------------------------------------------------------------------------------------
-------------------------------------------- MOSTRAR ASIGNACIONES ----------------------------

Alter proc sp_MostrarAsignacionesEstacionamiento
AS
BEGIN
	BEGIN TRY
	Select uiRegistroCajones, L.sLugar, CLD.sLugar AS LugarDireccion, sNombre, sCorreo,
	uiNumeroEmpleado, sRol, dtFechaAsignacion, sObservaciones
	from cajonesEstacionamientos CE
	INNER JOIN catLugares L ON CE.iCajon = L.uiRegistroCatalogo
	INNER JOIN usuarios U ON CE.iUsuario = U.IdUsuarios
	INNER JOIN roles R ON U.iRol = R.uiRegistroRol
	INNER JOIN catNumeroLugares CLD ON CE.iLugar = CLD.uiRegistroLugar
	Where U.iRol = 1 and CE.bActivo = 1 
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

Exec sp_MostrarAsignacionesEstacionamiento


-----------------------------------------------------------------------------------------------
-------------------------------------------- MOSTRAR ASIGNACIONES VISITANTES ----------------------------

Alter proc sp_MostrarAsignacionesEstacionamientoVisitantes
AS
BEGIN
	BEGIN TRY
	Select uiRegistroCajones, L.sLugar, CLD.sLugar AS LugarDireccion, dtFechaVisita,horaVisita,horaSalida, U.sNombre,
	UR.sNombre as NombreResponsable, sPlaca, sModelo, sColor, sRol, dtFechaAsignacion, sObservaciones
	from cajonesEstacionamientos CE
	INNER JOIN catLugares L ON CE.iCajon = L.uiRegistroCatalogo
	INNER JOIN usuarios U ON CE.iUsuario = U.IdUsuarios
	INNER JOIN roles R ON U.iRol = R.uiRegistroRol
	INNER JOIN usuarios UR ON U.idUserResponsables = UR.IdUsuarios
    INNER JOIN catNumeroLugares CLD ON CE.iLugar = CLD.uiRegistroLugar
	Where U.iRol = 2 and CE.bActivo = 1
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

Exec sp_MostrarAsignacionesEstacionamientoVisitantes

-------------------------------------------------------------------------------------------------------------
------------------------------------------- MOSTRAR ASIGNACIONES VISITANTES SIN LUGAR ----------------------------

Alter proc sp_MostrarAsignacionesEstacionamientoVisitantesSinLugar
AS
BEGIN
	BEGIN TRY
	Select uiRegistroCajones, L.sLugar, CLD.sLugar AS LugarDireccion, dtFechaVisita,horaVisita,horaSalida, U.sNombre,
	UR.sNombre as NombreResponsable, sPlaca, sModelo, sColor, sRol, dtFechaAsignacion, sObservaciones
	from cajonesEstacionamientos CE
	LEFT JOIN catLugares L ON CE.iCajon = L.uiRegistroCatalogo
	INNER JOIN usuarios U ON CE.iUsuario = U.IdUsuarios
	INNER JOIN roles R ON U.iRol = R.uiRegistroRol
	INNER JOIN usuarios UR ON U.idUserResponsables = UR.IdUsuarios
    LEFT JOIN catNumeroLugares CLD ON CE.iLugar = CLD.uiRegistroLugar
	Where U.iRol = 2 and CE.bActivo = 1 and CE.iCajon = 0 and CE.iLugar = 0
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

Exec sp_MostrarAsignacionesEstacionamientoVisitantesSinLugar

-----------------------------------------------------------------------------------------------
-------------------------------------------- VALIDAR USUARIO EMPLEADO ----------------------------
Alter Proc sp_ValidarUsuarioEmpelado
@uiNumeroEmpleado int
AS
BEGIN
	BEGIN TRY
	If Exists(Select * from usuarios where uiNumeroEmpleado = @uiNumeroEmpleado and iRol = 1 and bActivo = 1)
	Select 1
	ELSE
	Select 0
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

Select * from usuarios
delete from usuarios where uiNumeroEmpleado = 599

Select * from catLugares
Select * from catNumeroLugares where bActivo = 0
Select * from cajonesEstacionamientos


update usuarios set bActivo = 0 where IdUsuarios = 29


Select * from usuarios where uiNumeroEmpleado = 599
Select * from cajonesEstacionamientos where iUsuario = 99

------------------------------------------------------------------------------------------
------------------------------------ Update estacionamiento Libera ------------------------

Create proc sp_UpdateEstacionamiento
@uiRegistroCajones int
AS
BEGIN
	BEGIN TRY
	Update cajonesEstacionamientos set bActivo = 0 Where uiRegistroCajones = @uiRegistroCajones
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

------------------------------------ Update Lugares estacionamiento Libera ------------------------

Alter proc sp_UpdateLugaresEstacionamiento
@uiRegistroLugar int
AS
BEGIN
	BEGIN TRY
	Update catNumeroLugares set bActivo = 1 Where uiRegistroLugar = @uiRegistroLugar
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END


------------------------------------ Update Usuarios estacionamiento Libera ------------------------

Create proc sp_UpdateUsuarios
@IdUsuarios int
AS
BEGIN
	BEGIN TRY
	Update usuarios set bActivo = 0 Where IdUsuarios = @IdUsuarios
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END


Select * from cajonesEstacionamientos
Select * from catNumeroLugares where bActivo = 0

Select * from usuarios

Select * from catNumeroLugares where uiRegistroLugar = 154

-----------------------------------------------------------------------------------------------
------------------------------------ SP update HoraSalida -------------------------------------

Create proc sp_UpdateHoraSalida
@horaSalida varchar(50),
@uiRegistroCajones int
AS
BEGIN
	BEGIN TRY
	Update cajonesEstacionamientos set horaSalida = @horaSalida where uiRegistroCajones = @uiRegistroCajones
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

-----------------------------------------------------------------------------------------------
------------------------------------ SP update Asignar Cajones -------------------------------------

Alter proc sp_UpdateAsignarCajon
@iCajon int,
@uiRegistroCajones int
AS
BEGIN
	BEGIN TRY
	Update cajonesEstacionamientos set iCajon = @iCajon where uiRegistroCajones = @uiRegistroCajones
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

Select * from cajonesEstacionamientos

-------------------------------------------------------------------------------------------------
------------------------------------ SP update Asignar Lugares de Cajones -------------------------------------

Alter proc sp_UpdateAsignarCajonLugar
@iLugar int,
@uiRegistroCajones int
AS
BEGIN
	BEGIN TRY
	Update cajonesEstacionamientos set iLugar = @iLugar where uiRegistroCajones = @uiRegistroCajones
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END
----------------------------------------------------------------------------------------------
------------------------------------ SP Obtener fechas y horas sistema ------------------------

Alter proc sp_LiberarCajonesVisitantes
AS
Declare @tablaCajones table (registroCajon int, lugar int, fechaVisita DATE, horaSalida varchar(50), usuario int)
Declare @HoraSystem time,@FechaSystem varchar(100)
SET @HoraSystem = (SELECT CONVERT(time, GETDATE()))
SET @FechaSystem = (SELECT CONVERT(Date, GETDATE()))

insert into @tablaCajones (registroCajon, lugar, fechaVisita, horaSalida, usuario) Select uiRegistroCajones,iLugar, dtFechaVisita, horaSalida, iUsuario from cajonesEstacionamientos where dtFechaVisita =  @FechaSystem and bActivo = 1 and iLugar != 0
Declare @countCajones int = (Select COUNT(*) from @tablaCajones)

While @countCajones > 0
BEGIN
	
	declare @horaFinal varchar(50) = (select top(1) horaSalida from @tablaCajones) 
    declare @id int = (select top(1) registroCajon from @tablaCajones)
	declare @horaFinalCompara time = CONVERT(time,@horaFinal)
	declare @lugarCajon int = (select top(1) lugar from @tablaCajones)
	declare @usuarioCajon int = (select top(1) usuario from @tablaCajones)

	IF @HoraSystem >= @horaFinalCompara
	BEGIN
	update cajonesEstacionamientos set bActivo = 0 where uiRegistroCajones = @id
	update catNumeroLugares set bActivo = 1 where uiRegistroLugar = @lugarCajon
	update usuarios set bActivo = 0 where IdUsuarios = @usuarioCajon
	END
	
	delete @tablaCajones where registroCajon=@id	

	set @countCajones = (select count(*) from @tablaCajones)
END

Exec sp_LiberarCajonesVisitantes
---------------------------------------------------------------------------------------------------------

Select * from cajonesEstacionamientos
Exec sp_LiberarCajonesVisitantes

SELECT CONVERT(time, SYSDATETIME())
SELECT CONVERT(Date, SYSDATETIME())


	SET @HoraSystem = (SELECT CONVERT(nvarchar(5),GETDATE(), 108))

Select GETDATE() 

Select * from cajonesEstacionamientos where uiRegistroCajones = 17
Select * from catNumeroLugares where uiRegistroLugar = 112

update cajonesEstacionamientos set bActivo = 1 where uiRegistroCajones = 17

Select * from cajonesEstacionamientos where dtFechaAsignacion = '2021-03-22'

Select * from catNumeroLugares where uiRegistroLugar = 112
Select * from catNumeroLugares where uiRegistroLugar = 152


SELECT SYSDATETIME()
    ,CURRENT_TIMESTAMP
    ,GETDATE();

	---------------------------------------------------
	


Declare @tablaCajones table (registroCajon int, lugar int, fechaVisita DATE, horaSalida varchar(50))
Declare @HoraSystem time,@FechaSystem varchar(100)
SET @HoraSystem = (SELECT CONVERT(time, GETDATE()))
SET @FechaSystem = (SELECT CONVERT(Date, GETDATE()))

insert into @tablaCajones (registroCajon, lugar, fechaVisita, horaSalida) Select uiRegistroCajones,iLugar, dtFechaAsignacion, horaSalida from cajonesEstacionamientos where dtFechaAsignacion =  @FechaSystem  and bActivo = 1 and iLugar != 0
Declare @countCajones int = (Select COUNT(*) from @tablaCajones)

While @countCajones > 0
BEGIN
	
	declare @horaFinal varchar(50) = (select top(1) horaSalida from @tablaCajones) 
    declare @id int = (select top(1) registroCajon from @tablaCajones)
	declare @horaFinalCompara time = CONVERT(time,@horaFinal)
	declare @lugarCajon int = (select top(1) lugar from @tablaCajones)

	IF @HoraSystem >= @horaFinalCompara
	BEGIN
	update cajonesEstacionamientos set bActivo = 0 where uiRegistroCajones = @id
	update catNumeroLugares set bActivo = 1 where uiRegistroLugar = @lugarCajon
	END
	
	delete @tablaCajones where registroCajon=@id	

	set @countCajones = (select count(*) from @tablaCajones)
END


--------------------------------- Other codigo --------------------------------

Declare @HoraSystem varchar(100),@FechaSystem varchar(100), @fechaCajones Date, @horaSalida varchar(50), 
@uiRegistroCajon int, @iLugar int, @mensaje varchar(50), @horaFinalSystema time
BEGIN

	BEGIN TRY
	--SELECT SYSDATETIME()
	--SET @HoraSystem = (SELECT CONVERT(time(5),GETDATE(), 108))
	SET @HoraSystem = (SELECT CONVERT(time, GETDATE()))
	SET @FechaSystem = (SELECT CONVERT(Date, GETDATE()))
	SET @fechaCajones = (Select dtFechaAsignacion from cajonesEstacionamientos where dtFechaAsignacion =  @FechaSystem)
	--SET @horaSalida = (Select horaSalida from cajonesEstacionamientos where dtFechaAsignacion =  @FechaSystem)
	SET @horaSalida = (Select horaSalida from cajonesEstacionamientos where dtFechaAsignacion =  @FechaSystem)
	SET @uiRegistroCajon = (Select uiRegistroCajones from cajonesEstacionamientos where dtFechaAsignacion =  @FechaSystem)
	SET @iLugar = (Select iLugar from cajonesEstacionamientos where dtFechaAsignacion =  @FechaSystem)
	SET @horaFinalSystema = CONVERT(time,@horaSalida)

	Select @HoraSystem, @FechaSystem, @fechaCajones, @horaSalida,  @uiRegistroCajon, @iLugar, @horaFinalSystema

	IF @HoraSystem >= @horaFinalSystema
	update catNumeroLugares set bActivo = 1 where uiRegistroLugar = @iLugar
	IF @HoraSystem >= @horaFinalSystema
	update cajonesEstacionamientos set bActivo = 0 where uiRegistroCajones = @uiRegistroCajon
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

---------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------- POWER BI --------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------

-------------------------------------------- MOSTRAR ASIGNACIONES ----------------------------

Alter view view_MostrarAsignacionesEstacionamiento
AS		
	Select uiRegistroCajones, L.sLugar, CLD.sLugar AS LugarDireccion, iUsuario, U.sNombre, U.sCorreo, R.sRol, sObservaciones, 
	dtFechaAsignacion, dtFechaVisita,horaVisita,horaSalida, CE.bActivo, sPlaca, sModelo, sColor, UR.sNombre as NombreResponsable
	from cajonesEstacionamientos CE
	INNER JOIN catLugares L ON CE.iCajon = L.uiRegistroCatalogo
	INNER JOIN catNumeroLugares CLD ON CE.iLugar = CLD.uiRegistroLugar
	INNER JOIN usuarios U ON CE.iUsuario = U.IdUsuarios
	INNER JOIN roles R ON U.iRol = R.uiRegistroRol
	LEFT JOIN usuarios UR ON U.idUserResponsables = UR.IdUsuarios
	Where CE.bActivo = 1

GO

Select * from view_MostrarAsignacionesEstacionamiento

Select * from cajonesEstacionamientos where bActivo = 1

-------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------

-------------------------------------------- MOSTRAR ASIGNACIONES HISTORIAL ----------------------------

Alter view view_MostrarAsignacionesEstacionamientoHistorial
AS	
	Select uiRegistroCajones, iCajon, L.sLugar, CLD.sLugar AS LugarDireccion, iUsuario, U.sNombre, U.sCorreo, R.sRol, sObservaciones, 
	dtFechaAsignacion, dtFechaVisita,horaVisita,horaSalida, CE.bActivo, sPlaca, sModelo, sColor, UR.sNombre as NombreResponsable
	from cajonesEstacionamientos CE
	INNER JOIN catLugares L ON CE.iCajon = L.uiRegistroCatalogo
	INNER JOIN catNumeroLugares CLD ON CE.iLugar = CLD.uiRegistroLugar
	INNER JOIN usuarios U ON CE.iUsuario = U.IdUsuarios
	INNER JOIN roles R ON U.iRol = R.uiRegistroRol
	LEFT JOIN usuarios UR ON U.idUserResponsables = UR.IdUsuarios
	Where CE.bActivo = 0
GO

Select * from view_MostrarAsignacionesEstacionamientoHistorial
--------------------------------------------------------------------------------------------------------------

Select * from catLugares

Select * from catNumeroLugares where iLugar = 8 and sLugar = '15'

--alter table catNumeroLugares add bPresidencia bit



