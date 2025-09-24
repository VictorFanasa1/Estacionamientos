Use Estacionamiento




----------------- EDITAR CAJON ESTACIONAMIENTO -----------------

Select * from cajonesEstacionamientos where dtFechaVisita = '2021/07/06'

Select *  from catLugares

Select * from cajonesEstacionamientos where uiRegistroCajones = 517
Select * from catNumeroLugares where uiRegistroLugar = 221

Select * from catNumeroLugares where iLugar = 2 and bVisitante = 1 and sLugar = '72'


Select * from catLugares


------------------------------------ VALIDAR USUARIOS POR CAJÓN --------------

Select * from cajonesEstacionamientos where dtFechaVisita = '2021/07/30'

Select * from usuarios where IdUsuarios = 906
Select * from usuarios where IdUsuarios = 905 

Select * from usuarios where IdUsuarios = 900
Select * from usuarios where IdUsuarios = 899

Select * from usuarios where IdUsuarios = 918
Select * from usuarios where IdUsuarios = 917


Select * from usuarios where sNombre = 'DIANA SAMANTHA ROMERO MONTAÑO' and iRol = 3

Select * from usuarios where idUserResponsables = 923


Select * from cajonesEstacionamientos where dtFechaVisita = '2021/10/29'

Select * from usuarios where IdUsuarios = 930
Select * from usuarios where IdUsuarios = 929

Select * from cajonesEstacionamientos where iCajon = 0 and iLugar = 0

Select * from usuarios where sNombre = 'DIANA SAMANTHA ROMERO MONTAÑO' and iRol = 3


Select * from usuarios where idUserResponsables = 899 and bActivo = 1
Select * from usuarios where idUserResponsables = 905 and bActivo = 1

Select A.IdUsuarios, A.sNombre,  A.bActivo, A.idUserResponsables,
B.IdUsuarios,B.uiNumeroEmpleado, B.sNombre
from usuarios A
INNER JOIN usuarios B ON A.idUserResponsables = B.IdUsuarios
Where A.bActivo = 1 and B.uiNumeroEmpleado = 26714

-----------------------------    
---------------------------------

------------------------ Liberar Lugar Activo ---------------------
-------------------------------------------------------------------

Create proc sp_LiberarLugarActivo
@uiRegistroLugar int
AS
BEGIN

	BEGIN TRY
	UPDATE catNumeroLugares SET bActivo = 1 where uiRegistroLugar = @uiRegistroLugar
	END TRY


	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END


------------------------------------------- VALIDAR SOLIICTUDES --------------
------------------------------------------------------------------------------

Create proc sp_ValidarSolicitudesActivasUser
@uiNumeroEmpleado int
AS
BEGIN
	BEGIN TRY
	Select A.IdUsuarios, A.sNombre, A.iRol, A.bActivo, A.idUserResponsables,
	B.IdUsuarios,B.uiNumeroEmpleado, B.sNombre, B.iRol
	from usuarios A
	INNER JOIN usuarios B ON A.idUserResponsables = B.IdUsuarios
	Where A.bActivo = 1 and B.uiNumeroEmpleado = @uiNumeroEmpleado
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END

EXEC  sp_ValidarSolicitudesActivasUser 900942