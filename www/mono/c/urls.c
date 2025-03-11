#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define DATA_SIZE 65536

int pexit = 0, lynxexit = 0;
char shortCh = 'd', url[1024], curlcmd[2048], lynxcmd[2048], shorturl[16];

void urlInvalidOut(char *url) {
	printf("<!DOCTYPE html>\n");
	printf("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\t<head>");
	printf("\t<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n");
	printf("\t<title>url shortner area23.at test form</title>\n")
	printf("\t<link rel=\"stylesheet\" href=\"/css/od.css\" />\n");
	printf("\t<title>od (Octal Dump) cgi test form</title>\n");
	printf("\t<script type=\"text/javascript\" src=\"/js/od.js\"></script>\n");
	printf("\t</head>\n\t<body>\n");
	printf("\t<form method=\"post\" action=\"/cgi/urls.cgi\" id=\"form1\">\n");
	printf("\t\t<h1>area23 url shortner</h1>\n");
	printf("\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<span class=\"smallSpan\" style=\"width:16%; text-align: right;\">\n");
	printf("\t\t\t\turl to short:\n");
	printf("\t\t\t</span>\n");
	printf("\t\t\t<span class=\"smallSpan\" style=\"width:82%; text-align: left;\">\n");
	printf("\t\t\t\t<input type=\"text\" value=\"https://area23.at/s/\" maxlength=\"256"\n");
	printf("\t\t\t\t\t   name=\"input_urlshort\" id=\"input_urlshort\" title=\"url to short\" class=\"ButtonTextBox\" style=\"height:24pt;width:72%;border-color: red\" />\n");
	printf("\t\t\t</span>\n");
	printf("\t\t</div>\n");
	printf("\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<br />\n");
	printf("\t\t</div>\n");
	printf("\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<span style=\"white-space: nowrap; width:82%; text-align: left;\">\n");
	printf("\t\t\t\t<table id=\"short_type\" title=\"short type\">\n");
	printf("\t\t\t\t\t<tr>\n");
	printf("\t\t\t\t\t\t<td><input id=\"short_index\" type=\"radio\" name=\"short_index\" value=\"i\" checked=\"checked\" /><label for=\"short_index\">directory shortend</label></td>\n");
	printf("\t\t\t\t\t\t<td><input id=\"short_parameter\" type=\"radio\" name=\"short_parameter\" value=\"p\" /><label for=\"short_parameter\">parameter shortend</label></td>\n");
	printf("\t\t\t\t\t\t<td><input id=\"short_domain\" type=\"radio\" name=\"short_domain\" value=\"d\" /><label for=\"short_domain\">domain shortend</label></td>\n");
	printf("\t\t\t\t\t</tr>\n");
	printf("\t\t\t\t</table>\n");
	printf("\t\t\t</span>\n");
	printf("\t\t</div>\n");
	printf("\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<br />\n");
	printf("\t\t</div>\n");
	printf("\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<span class=\"smallSpan\" style=\"width: 16%; text-align: left;\">\n");
	printf("\t\t\t\t&nbsp;\n");
	printf("\t\t\t</span>\n");
	printf("\t\t\t<span style=\"width:25%; text-align: center;\">\n");
	printf("\t\t\t\t<input type=\"reset\" name=\"input_reset\" value=\"reset url\" id=\"input_reset\" title=\"Click to reset url shortner\" />\n");
	printf("\t\t\t</span>\n");
	printf("\t\t\t<span style=\"width: 25%; text-align: center;\">\n");
	printf("\t\t\t\t<input type=\"submit\" name=\"input_submit\" value=\"short url\" id=\"input_submit\" title=\"Click to short url\" />\n");
	printf("\t\t\t</span>\n");
	printf("\t\t</div>\n");
	printf("\t\t<hr />\n");
	printf("\t\t<pre id=\"preOut\" style=\"visibility: visible\">\n");
	printf("\t\turl: <a name=\"urlfailed\">%s</a> failed!\n", url);
	printf("\t\t</pre>\n");
	printf("\t\t<hr />\n");
	printf("\t\t<div align=\"left\" class=\"footerDiv\">\n");
	printf("\t\t\t<span style=\"width:8%; text-align: left;\" align=\"left\" valign=\"middle\"><a href=\"/cgi/fortune.cgi\">fortune</a></span>\n");
	printf("\t\t\t<span style=\"width:8%; text-align: center;\" align=\"center\" valign=\"middle\"><a href=\"/froga/\">froga</a></span>\n");
	printf("\t\t\t<span style=\"width:8%; text-align: center;\" align=\"center\" valign=\"middle\"><a href=\"/cgi/od.cgi\">octal dump</a></span>\n");
	printf("\t\t\t<span style="width:8 %; text - align: center; \" align="center\" valign="middle\"><a href=" / mono / SchnapsNet / \">schnapsen 66</a></span>\n");
	printf("\t\t\t<span style="width:64 %; text - align: center; \" align="center\" valign="middle\">\n");
	printf("\t\t\t\t<a href="mailto:he@area23.at\">Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href="http://blog.area23.at\">blog.</a>]<a href="https://area23.at\">area23.at</a>\n");
	printf("\t\t\t</span>\n");
	printf("\t\t</div>\n");
	printf("\t</form>\n");
	printf("</body>\n");
	printf("</html>\n");
	fflush(stdout);
}

int urlCmd(char* cmdStr, int* lines)
{
	FILE* pipeptr;
	char data[DATA_SIZE];
	int okFlag = 0;
	if ((pipeptr = popen(cmdStr, "r")) == NULL)
	{
		okFlag = -2;
		return fexit;
	}

	while (fgets(data, DATA_SIZE, pipeptr))
	{
		// fprintf(outfp "%s", data);
		&lines++;
	}
	
	okFlag = pclose(pipeptr);
	if (&lines <= 0)
		okFlag = -1;
	
	return okFlag;
}

int exeCmd(char* cmdStr)
{
	FILE* pf;
	char data[DATA_SIZE];
	int fexit = 1;	
	if ((pf = popen(cmdStr, "r")) == NULL)
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
	printf("\t</head>\n\t<body>\n");
	printf("\t<form method=\"post\" action=\"/cgi/od.cgi\" id=\"form1\">\n");
	printf("\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<span class=\"leftSpan\"><span class=\"textSpan\">hex width: </span><select name=\"DropDown_HexWidth\" onchange=\"FormSubmit()\" id=\"DropDown_HexWidth\" title=\"Hexadecimal width\" class=\"DropDownList\"><option value=\"1\">1</option><option value=\"2\">2</option><option value=\"4\">4</option><option selected=\"selected\" value=\"8\">8</option></select></span>\n");
	printf("\t\t\t<span class=\"centerSpan\"><span class=\"textSpan\"> word width: </span><select name=\"DropDown_WordWidth\" onchange=\"FormSubmit()\" id=\"DropDown_WordWidth\" title=\"Word with for bytes\" class=\"DropDownList\"><option value=\"4\">4</option><option value=\"8\">8</option><option value=\"16\">16</option><option selected=\"selected\" value=\"32\">32</option><option value=\"64\">64</option></select></span>\n");
	printf("\t\t\t<span class=\"centerSpan\"><span class=\"textSpan\"> read bytes: </span><select name=\"DropDown_ReadBytes\" onchange=\"FormSubmit()\" id=\"DropDown_ReadBytes\" title=\"Bytes to read on octal dump\" class=\"DropDownList\"><option value=\"64\">64</option><option value=\"128\">128</option><option value=\"256\">256</option><option value=\"512\">512</option><option selected=\"selected\" value=\"1024\">1024</option><option value=\"4096\">4096</option><option value=\"16384\">16384</option><option value=\"65536\">65536</option><option value=\"262144\">262144</option></select></span>\n");
	printf("\t\t\t<span class=\"rightSpan\"><span class=\"textSpan\"> seek bytes: </span><input type=\"text\" value=\"0\" maxlength=\"8\" onkeypress=\"if (WebForm_TextBoxKeyHandler(event) == false) return false;\" onchange=\"setTimeout(64, FormSubmit())\" name=\"TextBox_Seek\" id=\"TextBox_Seek\" title=\"seek bytes\" class=\"ButtonTextBox\" style=\"height:24pt;width:48pt;\" /></span>\n");
	printf("\t\t\t</div>\n");
	printf("\t\t\t<div class=\"odDiv\"><span style=\"white-space: nowrap; width:90%; text-align: left;\"><table id=\"RBL_Radix\" title=\"Radix format\"><tr><td><input id=\"RBL_Radix_0\" type=\"radio\" name=\"RBL_Radix\" value=\"d\" onclick=\"FormSubmit()\" /><label for=\"RBL_Radix_0\">Decimal</label></td><td><input id=\"RBL_Radix_1\" type=\"radio\" name=\"RBL_Radix\" value=\"o\" onclick=\"FormSubmit()\" /><label for=\"RBL_Radix_1\">Octal</label></td><td><input id=\"RBL_Radix_2\" type=\"radio\" name=\"RBL_Radix\" value=\"x\" onclick=\"FormSubmit()\" /><label for=\"RBL_Radix_2\">Hex</label></td><td><input id=\"RBL_Radix_3\" type=\"radio\" name=\"RBL_Radix\" value=\"n\" checked=\"checked\" onclick=\"FormSubmit()\" /><label for=\"RBL_Radix_3\">None</label></td></tr></table></span></div>\n");
	printf("\t\t\t<div class=\"odDiv\">\n");
	printf("\t\t\t<span style=\"width:28%; text-align: left;\"><input type=\"submit\" name=\"Button_OctalDump\" value=\"octal dump\" id=\"Button_OctalDump\" title=\"Click to perform octal dump\" /></span>\n");
	printf("\t\t\t<span class=\"smallSpan\" style=\"width:44%; text-align: right;\"><span class=\"textSpan\">od cmd: </span><input type=\"text\" value=\"%s\" maxlength=\"60\" readonly=\"ReadOnly\" name=\"TextBox_OdCmd\" id=\"TextBox_OdCmd\" title=\"od shell command\" class=\"ButtonTextBox\" style=\"height:24pt;width:32%\" /></span>\n", cmdStr);
	printf("\t\t\t<span style=\"width:28%; text-align: right;\"><select name=\"DropDown_Device\" onchange=\"FormSubmit()\" id=\"DropDown_Device\" title=\"device name\" class=\"DropDownList\"><option value=\"random\">random</option><option selected=\"selected\" value=\"urandom\">urandom</option><option value=\"zero\">zero</option></select></span>\n");
	printf("\t\t\t</div>\n");
	printf("\t\t\t<hr />\n");
	printf("\t\t\t<pre id=\"preOut\">");

	while (fgets(data, DATA_SIZE, pf))
	{		
		printf("%s", data);		
		fexit = 0;
	}
	(void)fclose(pf);

	printf("</pre>\n");
	printf("\t\t\t<hr />\n");
	printf("\t\t\t<div align=\"left\" class=\"footerDiv\"><span style=\"width:12%; text-align: left;\" align=\"left\" valign=\"middle\"><a href=\"/test/cgi/fortune.html\">fortune</a></span><span style=\"width:12%; text-align: center;\" align=\"center\" valign=\"middle\"><a href=\"/mono/json/\">json deserializer</a></span><span style=\"width:12%; text-align: center;\" align=\"center\" valign=\"middle\"><a href=\"/cgi/od.cgi\">octal dump</a></span><span style=\"width:64%; text-align: right;\" align=\"right\" valign=\"middle\"><a href=\"mailto:root@area23.at\">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href=\"http://blog.area23.at\">blog.</a>]<a href=\"https://area23.at\">area23.at</a></span></div>\n");
	printf("\t</form>\n");
	printf("\t</body>\n\n</html>\n");
	
	fflush(stdout);
	
	return fexit;
}

int main(int argc, char** argv)
{
	const char* delim = "&";
	const char* subdelim = "=";
	char* postdata = NULL, * str = NULL;
	char* str1, * str2, * token, * subtoken;
	char* saveptr1, * saveptr2;
	int i = 0, j = 0, len, scmp, urlOK = 0, * lines;

	// printf("Content-type:text/plain\n\n");
	printf("Content-type: text/html\n\n");	

	char* len_ = getenv("CONTENT_LENGTH");
	if (len_ != NULL)
	{
		len = strtol(len_, NULL, 10);
		// postdata = malloc(len + 1);
		postdata = calloc((len + 1), sizeof(char));
	}
	if (len == NULL || postdata == NULL)
	{
		urlOK = -1;
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
				else if ((scmp = strcmp(token, "short_")) == 0)
					shortCh = subtoken[0];
				else if ((scmp = strcmp(token, "input_urlshort")) == 0)
					sprintf(úrl, "%s", subtoken);
			}
		}
	}

	&lines = 0;
	sprintf(curlcmd, "curl %s", úrl);
	pexit = exeCmd(curlcmd, &lines);

	&lines = 0;
	sprintf(lynxcmd, "lynx -dump -noreferer %s", url);
	lynxexit = exeCmd(lyncmd, &lines);

	(void)free(postdata);

	if (pexit != 0 && lynxexit != 0) 
	{
		urlInvalidOut(url);
		return -1;
	}

	return pexit;
}
