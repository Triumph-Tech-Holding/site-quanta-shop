/*************************************************************************
Autor: Proaire / Marcelo Drumand
Data : 18/12/2019
Descrição: Script de criação de procedure que traz pedidos tanto para usuarios quando para admim

***************************************************************************/
PRINT '***********************************************************************';
PRINT 'Cabeçalho de execução de script - Equipe Proaire Banco de Dados';
PRINT 'Instancia   = ' + @@SERVERNAME;
PRINT 'Database    = ' + DB_NAME();
PRINT 'Data e Hora = ' + CONVERT(VARCHAR(30), GETDATE(), 121);
PRINT 'Login       = ' + SYSTEM_USER;
PRINT '***********************************************************************';

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/****** Object:  Procedure spc_ValoresHome   Script Date: 18/12/2019 14:00:42 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_Pedidos')
   DROP PROCEDURE spc_Pedidos     
GO

-- =============================================  
-- Author: Marcelo Drumand   
-- Create date: 18-12-2019   
-- Description: Este Procedimento irá trazer para tela todos os pedidos, para usuario e para Admin  
-- spc_Pedidos null, null, null, 2
-- =============================================  
Create PROCEDURE spc_Pedidos  
(  
 @IdUsuario varchar(255) = Null,   
 @DataInicio datetime = NULL,  
 @DataFim datetime = NULL,   
 @StatusTransacao int = NULL  
)  
AS  
BEGIN  
 SELECT p.IdPedido
	   ,p.Codigo AS CodidoPedido
	   ,p.IdUsuario
	   ,prod.Nome AS NomeProduto
	   ,p.ValorPedido
	   ,p.ValorTaxa  
	   ,p.DataPedido
	   ,s.Nome As StatusTransacao
	   ,p.EnderecoDeposito
	   ,p.UrlPagamento
	   ,p.Cotacao
 FROM Pedido p
  INNER JOIN Transacao t ON p.idTransacao = t.IdTransacao
  INNER JOIN UsuarioProduto up ON up.Idpedido = p.idPedido 
  INNER JOIN Produto prod ON prod.idProduto = up.idProduto
  INNER JOIN Status s ON s.IdStatus = t.IdStatus  
 WHERE p.DataPedido BETWEEN ISNULL(@DataInicio, p.DataPedido) AND ISNULL(@DataFim, p.DataPedido)  
  AND p.Idusuario = ISNULL(CAST(@IdUsuario as uniqueidentifier), p.Idusuario)  
  AND t.IdStatus = ISNULL(@StatusTransacao, t.IdStatus)  
  AND p.Ativo = 1
END
GO
PRINT 'PROCEDIMENTO CRIADO'

