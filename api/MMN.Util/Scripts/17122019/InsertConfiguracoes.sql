 /*************************************************************************
Autor: Proaire / Marcelo Drumand
Data : 09/11/2019
Descrição: Inserindo Campo Status na tabela lancamento e valores do status na tabela Tipo

***************************************************************************/
USE [atlantic-wonder]

PRINT '***********************************************************************';
PRINT 'INSERT CONFIGURAÇÕES ';
PRINT 'Instancia   = ' + @@SERVERNAME;
PRINT 'Database    = ' + DB_NAME();
PRINT 'Data e Hora = ' + CONVERT(VARCHAR(30), GETDATE(), 121);
PRINT 'Login       = ' + SYSTEM_USER;
PRINT '***********************************************************************';


IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'TAXA_COMPRA')
 INSERT INTO Configuracao VALUES ('TAXA_COMPRA', '0,01', 'Taxa de processamento', 1);
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'URL_BASE')
 INSERT INTO Configuracao VALUES ('URL_BASE', 'http://localhost:8080', 'Url Base da aplicação VUE',	1);
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'MATRIZ_CONFERE_EXISTENCIA')
 INSERT INTO Configuracao VALUES ('MATRIZ_CONFERE_EXISTENCIA', '1','Permite inserção duplicada na matriz', 1);
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'USAR_CRYSTALPAY')
 INSERT INTO Configuracao VALUES ('USAR_CRYSTALPAY', 'true','Utiliza o método de pagamento CrystalPay', 1);
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'CRYSTALPAY_APIKEY')
 INSERT INTO Configuracao VALUES ('CRYSTALPAY_APIKEY', 'B3eMc32D0az72URqlV0J9uyTPjeZeOm05QXow8kL6W8FE4d1d1','KEY CrystalPay', 1);
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'CRYSTALPAY_SECRET')
 INSERT INTO Configuracao VALUES ('CRYSTALPAY_SECRET', '0mubZ01edFMHce9AGRBXzKL5fCWy9NpjJ47814ekV0iqvSETwg','Secret CrystalPay', 1);
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'COTACAO_DOLAR')
INSERT INTO Configuracao VALUES ('COTACAO_DOLAR', '4,50', 'Cotação base para conversão do dolar', 1)
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'PERCENTUAL_CASHBACK_DIARIO')
INSERT INTO Configuracao VALUES ('PERCENTUAL_CASHBACK_DIARIO', '0,7', 'Valor de retorno diário de cashback', 1)
GO

IF NOT EXISTS (SELECT * FROM Configuracao WHERE Chave = 'TAXA_SAQUE')
INSERT INTO Configuracao values ('TAXA_SAQUE', '0,08', 'Taxa padrão de saque', 1)
GO