Use Estacionamiento

------------------------------------------------PRUEBAS -----------------------------------------
------------------------------------------------------------------------------------------------
Select * from cajonesEstacionamientos where uiRegistroCajones = 529

Select * from usuarios where IdUsuarios = 904
Select * from usuarios where IdUsuarios = 903


Select * from cajonesEstacionamientos where iCajon = 0

Select * from roles


delete from usuarios where IdUsuarios = 901

delete from cajonesEstacionamientos where uiRegistroCajones = 528

---------------------------------------------------------------------------------


sp_helptext sp_MostrarAsignacionesEstacionamientoVisitantes

----------------------------------------------------------- ANEXOS -----------------------------------

--------------------------------------------- sp cajón visitante único ------------------------------------

CREATE proc sp_MostrarAsignacionesEstacionamientoVisitantesUnico
@uiRegistroCajones int
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
	Where U.iRol = 2 and CE.bActivo = 1 and CE.uiRegistroCajones = @uiRegistroCajones
	END TRY

	BEGIN CATCH
	Declare @ErrorProcedure varchar(500), @ErrorMessage varchar(500), @MSG varchar(100);
	Select @ErrorProcedure = ERROR_PROCEDURE(), @ErrorMessage=ERROR_MESSAGE();
	Select @MSG = ERROR_PROCEDURE() + '|' + ERROR_MESSAGE();
	THROW 53000, @MSG, 1
	END CATCH
END
