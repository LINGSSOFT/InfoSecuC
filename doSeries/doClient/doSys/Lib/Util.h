#ifndef __UTIL_H__
#define __UTIL_H__

void ViewLog(int nLog);
void RemoveLog(int nLog);
void WriteLog(int nLog, char *fmt,...);
void WriteLog2(char *fname, char *fmt,...);
char * GetErrString(char *msg, DWORD msg_size, DWORD err);
int GetFilePath(char *fname, char *path);
int getOSInfo();

#endif
