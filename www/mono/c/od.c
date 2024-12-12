#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define DATA_SIZE 65536
#define QDAT_SIZE 262144 
#define MEGA_SIZE 1048576  

int hexwidth = 8, wordwidth = 32, readbytes = 1024, seekbytes = 0, pexit = 0;
char radix = 'n', device[64], odcmd[1024], odraw[QDAT_SIZE];
const char* dumpfile = "/tmp/dumpfile.txt";

int exeCmd(char* cmdStr)
{
	FILE* pf;
	char data[MEGA_SIZE];
	int fexit = 1;	
	if ((pf = popen(odcmd, "r")) == NULL)
	{
		fexit = 2;
		return fexit;
	}

	printf("<!DOCTYPE html>\n");
	printf("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\t<head>");
	printf("\t<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n");
	printf("\t<link rel=\"stylesheet\" href=\"/css/od.css\" />\n");
	printf("\t<title>od (Octal Dump) cgi test form</title>\n");
	printf("\t<script type=\"text/javascript\" src=\"/js/od.js\"></script>\n");
	printf("\t</head>\n");
	printf("\t<body>\n");
	printf("\t<form method=\"post\" action=\"/cgi/od.cgi\" id=\"form1\">\n");
	
	printf("\t\t<div class=\"odDiv\">\n");
	
	printf("\t\t\t<span class=\"leftSpan\">");
	printf("<span class=\"textSpan\">hex width: </span>");
	printf("<select name=\"DropDown_HexWidth\" onchange=\"OdFormSubmit()\" ");	
	printf("id=\"DropDown_HexWidth\" title=\"Hexadecimal width\" class=\"DropDownList\">");
	printf("<option value=\"1\">1</option>");
	printf("<option value=\"2\">2</option>");
	printf("<option value=\"4\">4</option>");
	printf("<option selected=\"selected\" value=\"8\">8</option></select></span>\n");
	printf("\t\t\t<span class=\"centerSpan\"><span class=\"textSpan\"> word width: </span>");
	printf("<select name=\"DropDown_WordWidth\" onchange=\"OdFormSubmit()\" id=\"DropDown_WordWidth\" title=\"Word with for bytes\" class=\"DropDownList\">");
	printf("<option value=\"4\">4</option>");
	printf("<option value=\"8\">8</option>");
	printf("<option value=\"16\">16</option>");
	printf("<option selected=\"selected\" value=\"32\">32</option>");
	printf("<option value=\"64\">64</option></select></span>\n");
	printf("\t\t\t<span class=\"centerSpan\">");
	printf("<span class=\"textSpan\"> read bytes: </span>");
	printf("<select name=\"DropDown_ReadBytes\" onchange=\"OdFormSubmit()\" id=\"DropDown_ReadBytes\" title=\"Bytes to read on octal dump\" class=\"DropDownList\"><option value=\"64\">64</option>");
	printf("<option value=\"128\">128</option><option value=\"256\">256</option>");
	printf("<option value=\"512\">512</option>");
	printf("<option selected=\"selected\" value=\"1024\">1024</option>");
	printf("<option value=\"4096\">4096</option>");
	printf("<option value=\"16384\">16384</option>");
	printf("<option value=\"65536\">65536</option><option value=\"262144\">262144</option></select></span>\n");
	printf("\t\t\t<span class=\"rightSpan\"><span class=\"textSpan\"> seek bytes: </span>");
	printf("<input type=\"text\" value=\"0\" maxlength=\"8\" onkeypress=\"if (WebForm_TextBoxKeyHandler(event) == false) return false;\" onchange=\"setTimeout(64, OdFormSubmit())\" name=\"TextBox_Seek\" id=\"TextBox_Seek\" title=\"seek bytes\" class=\"ButtonTextBox\" style=\"height:24pt;width:48pt;\" />");
	printf("</span>\n");
	
	printf("\t\t\t</div>\n");
	
	printf("\t\t\t<div class=\"odDiv\"><span style=\"white-space: nowrap; width: 90%; text-align: left;\">");
	printf("<table id=\"RBL_Radix\" title=\"Radix format\"><tr>");
	printf("<td><input id=\"RBL_Radix_0\" type=\"radio\" name=\"RBL_Radix\" value=\"d\" onclick=\"OdFormSubmit()\" /><label for=\"RBL_Radix_0\">Decimal</label></td>");
	printf("<td><input id=\"RBL_Radix_1\" type=\"radio\" name=\"RBL_Radix\" value=\"o\" onclick=\"OdFormSubmit()\" /><label for=\"RBL_Radix_1\">Octal</label></td>");
	printf("<td><input id=\"RBL_Radix_2\" type=\"radio\" name=\"RBL_Radix\" value=\"x\" onclick=\"OdFormSubmit()\" /><label for=\"RBL_Radix_2\">Hex</label></td>");
	printf("<td><input id=\"RBL_Radix_3\" type=\"radio\" name=\"RBL_Radix\" value=\"n\" checked=\"checked\" onclick=\"OdFormSubmit()\" /><label for=\"RBL_Radix_3\">None</label></td></tr></table>");
	printf("</span></div>\n");
	
	printf("\t\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<span style=\"width: 28%; text-align: left;\">");
	printf("<input type=\"submit\" name=\"Button_OctalDump\" value=\"octal dump\" id=\"Button_OctalDump\" title=\"Click to perform octal dump\" />");
	printf("</span>\n");
	
	printf("\t\t\t<span class=\"smallSpan\" style=\"width: 44%; text-align: right;\">");
	printf("<span class=\"textSpan\">od cmd: </span>");
	printf("<input type=\"text\" value=\"%s\" maxlength=\"60\" readonly=\"ReadOnly\" name=\"TextBox_OdCmd\" id=\"TextBox_OdCmd\" title=\"od shell command\" class=\"ButtonTextBox\" style=\"height:24pt; width: 32%\" />", cmdStr);
	printf("</span>\n");
	printf("\t\t\t<span style=\"text-align: right; width: 28%\">");
	printf("<select name=\"DropDown_Device\" onchange=\"OdFormSubmit()\" id=\"DropDown_Device\" title=\"device name\" class=\"DropDownList\">");
	printf("<option value=\"random\">random</option><option selected=\"selected\" value=\"urandom\">urandom</option>");
	printf("<option value=\"zero\">zero</option></select></span>\n");
	printf("\t\t\t</div>\n");
	
	printf("\t\t\t<hr />\n\t\t\t");
	printf("<textarea id=\"odRawText\" readonly=\"ReadOnly\" cols=\"48\" rows=\"10\" name=\"odraw\" title=\"octal dump raw text\" ");
	printf(" style=\"border-color:LightGrey;border-width:1px;border-style:Solid;scroll-behavior: smooth; scrollbar-widh: auto; width:480px;font-family: monospace, ui-monospace, system-ui, sans-serif;\">");
	
	while (fgets(data, DATA_SIZE, pf))
	{		
		printf("%s", data);		
		fexit = 0;
	}
	(void)fclose(pf);

	printf("</textarea>\n");
	printf("\t\t\t<hr />\n");
	printf("\t\t\t<div align=\"left\" class=\"footerDiv\"><span style=\"width: 12%; text-align: left;\" align=\"left\" valign=\"middle\">");
	printf("<a href=\"/cgi/fortune.cgi\">fortune</a></span>");
	printf("<span style=\"text-align: center; width: 12%\" align=\"center\" valign=\"middle\"><a href=\"/net/\">c# .net mono demo</a></span>");
	printf("<span style=\"width: 12%; text-align: center;\" align=\"center\" valign=\"middle\"><a href=\"/cgi/od.cgi\">octal dump</a></span><span style=\"width: 64%; text-align: right;\" align=\"right\" valign=\"middle\">");
	printf("<a href=\"mailto:zen@area23.at\">Heinrich Elsigan</a>, GNU General Public License 2.0, ");
	printf("[<a href=\"https://blog.area23.at\">blog.</a>]");
	printf("<a href=\"https://area23.at\">area23.at</a></span></div>\n");
	printf("\t</form>\n");
	printf("\t</body>\n\n</html>\n");
	
	fflush(stdout);
	
	return fexit;
}


int saveDump(char *rawdump) {
	FILE* fp;
	char data[MEGA_SIZE];
	char* reststr = rawdump;
	int fexit = 1;	

	
	if ((fp = fopen("/tmp/dumpfile.txt", "w+")) == NULL)
	{
		printf("<!-- Cannot open dumpfile: /tmp/dumpfile.txt -->\n");
		fexit = 2;
		return fexit;
	}
	
	printf("<!-- %s -->\n",  strchr(rawdump, (int)((char)'\n')));
	
	
	fprintf(fp, "%s\n\0", rawdump);
	fflush(fp);	
	/*
	while ((reststr = strchr(rawdump, (int)((char)'\n'))) != NULL) 
	{
			rawdump = reststr;
			fprintf(fp, "%s", rawdump);
	}	
	fprintf(fp, "%s\n", reststr);
	fflush(fp);
	*/
	fclose(fp);
	
	printf("<!-- raw dump finished -->\n");
	return 0;
	
}

int main(int argc, char** argv)
{
	const char* delim = "&";
	const char* subdelim = "=";
	char* postdata = NULL, * str = NULL;
	char* str1, * str2, * token, * subtoken;
	char* saveptr1, * saveptr2;
	char *ddraw;
	int i = 0, j = 0, len, scmp;

	// printf("Content-type:text/plain\n\n");
	printf("Content-type: text/html\n\n");	

	char* len_ = getenv("CONTENT_LENGTH");
	if (len_ != NULL)
	{
		len = strtol(len_, NULL, 10);
		// postdata = malloc(len + 1);
		postdata = calloc((len + 1), sizeof(char));
	}
	if (len_ == NULL || postdata == NULL)
	{
		sprintf(odcmd, "od -A %c -t x%dz -w%d -v -j%d -N%d /dev/urandom",
			radix, hexwidth, wordwidth, seekbytes, readbytes);
		return exeCmd(odcmd);
	}


	if ((ddraw = strstr(postdata, "odraw")) != NULL) 
	{
		saveDump(ddraw);		
	}
	
	while (fgets(postdata, len + 1, stdin)) 	/* work with postdata */
	{
		// printf("\nPOST DATA:\n%s\n", postdata);
		for (j = 1, str1 = postdata; ; j++, str1 = NULL)
		{
			token = strtok_r(str1, "&", &saveptr1);
			if (token == NULL)
				break;
			// printf("%d: \t%s\n", j, token);

			for (str2 = token; ; str2 = NULL)
			{
				subtoken = strtok_r(str2, "=", &saveptr2);
				if (subtoken == NULL)
					break;
				// printf("-->\t%s\n", subtoken);
				if ((scmp = strcmp(token, "DropDown_HexWidth")) == 0)
					hexwidth = strtol(subtoken, NULL, 10);
				else if ((scmp = strcmp(token, "DropDown_WordWidth")) == 0)
					wordwidth = strtol(subtoken, NULL, 10);
				else if ((scmp = strcmp(token, "DropDown_ReadBytes")) == 0)
					readbytes = strtol(subtoken, NULL, 10);
				else if ((scmp = strcmp(token, "TextBox_Seek")) == 0)
					seekbytes = strtol(subtoken, NULL, 10);
				else if ((scmp = strcmp(token, "RBL_Radix")) == 0)
					radix = subtoken[0];
				else if ((scmp = strcmp(token, "DropDown_Device")) == 0)
					sprintf(device, "/dev/%s", subtoken);
				else if ((scmp = strcmp(token, "odRawText")) == 0) 
				{
					// odstrcpy(odraw, subtoken); 
					sprintf(odraw, "%s", subtoken);	
					saveDump(odraw);
				}
					
									
			}
		}
	}

	sprintf(odcmd, "od -A %c -t x%dz -w%d -v -j%d -N%d %s",
		radix, hexwidth, wordwidth, seekbytes, readbytes, device);
	pexit = exeCmd(odcmd);

	(void)free(postdata);

	return pexit;
}
