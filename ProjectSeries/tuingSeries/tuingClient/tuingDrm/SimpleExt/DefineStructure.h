// DefineStructure.h
// define socket protocols

#define FIRST_CONN	1
#define BUFSIZE			512
#define MAXLENGTH		250
#define NAMESIZE	128
#define IPSIZE			64
#define MACSIZE		12

// Protocol for authentication 
typedef struct
  TQuestionData {
	TCHAR szCommand [MACSIZE];
	TCHAR szMAC	[MACSIZE+1];
	TCHAR szUser [IPSIZE+1];
	TCHAR szOS [IPSIZE+1];
	TCHAR szProcessor[IPSIZE+1];
	TCHAR szIP [IPSIZE+1];
	UINT iRAM ;
	UINT iHDD ;
	UINT iReservation;
  } *PTQuestionData;

// Protocol for printer's information 
typedef struct
  TPrinterData {
	TCHAR szCommand [MACSIZE];
	TCHAR szMAC	[MACSIZE+1];
	TCHAR szPrinter [IPSIZE+1];
	TCHAR szLocation [IPSIZE+1];
	TCHAR szModel[IPSIZE+1];
	UINT iColor;
	UINT iSpeed ;
	UINT iResolution;
	UINT iReservation;
  } *PTPrinterData;

// Protocol for printer's information 
typedef struct
  TSoftwareData {
	TCHAR szCommand [MACSIZE];
	TCHAR szMAC	[MACSIZE+1];
	TCHAR szDescription [IPSIZE+1];
	TCHAR szPublisher [IPSIZE+1];
	TCHAR szFileName [IPSIZE+1];
	TCHAR szVersion[IPSIZE+1];
	TCHAR szInstallDate[IPSIZE+1];
	UINT iReservation;
  } *PTSoftwareData;

// Protocol for print log 
typedef struct
  TPrintData {
	TCHAR szCommand [MACSIZE];
	TCHAR szMAC	[MACSIZE+1];
	TCHAR szUser [IPSIZE+1];
	TCHAR szProcess[NAMESIZE+1]; //20081110, change to the file name for capture 
	TCHAR szPrinter	[NAMESIZE+1];
	TCHAR szDoc	[NAMESIZE+1];
	UINT iPage  ;
	UINT iPaper;
	UINT iColor  ;	
	UINT iCopies  ;	
	UINT iQuality  ;	
  } *PPrintData;

// Protocol for application log 
typedef struct
  TApplicationLog {
	TCHAR szCommand [MACSIZE];
	TCHAR szMAC	[MACSIZE+1];
	TCHAR szImageName [IPSIZE+1];
	TCHAR szTitle [MAXLENGTH+1];
	TCHAR szTime [IPSIZE+1];
	UINT iType;
	UINT iReservation;
  } *PApplicationLog;

// Protocol for url log 
typedef struct
  TURLLog {
	TCHAR szCommand [MACSIZE];
	TCHAR szMAC	[MACSIZE+1];
	TCHAR szUrl [NAMESIZE+1];
	TCHAR szTitle [MAXLENGTH+1];
	TCHAR szTime [IPSIZE+1];
	UINT iType;
	UINT iReservation;
  } *PURLLog;

// Protocol for file log 
typedef struct
  TFILELog {
	TCHAR szCommand [MACSIZE];
	TCHAR szMAC	[MACSIZE+1];
	TCHAR szFile [MAXLENGTH+1];
	TCHAR szTime [IPSIZE+1];
	UINT iType;
	UINT iReservation;
  } *PFILELog;

// command packet to send to the client 
typedef struct
  TCommData {
	TCHAR szCommand [MACSIZE];
	TCHAR szData1 [10][IPSIZE+1];
	UINT iData1  ; // server IP
	UINT iData2  ; // flag for watermark, footer mark, capture, print restriction
	UINT iData3  ; // collect command
	UINT iData4  ; // period value for server connection
	UINT iData5  ; // not use
	UINT iData6  ; // not use
  } *PCommData;