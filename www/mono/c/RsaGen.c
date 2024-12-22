#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define DATA_SIZE 65536
#define QDAT_SIZE 262144
#define MEGA_SIZE 1048576

int hexwidth = 1024, wordwidth = 32, readbytes = 1024, seekbytes = 0, pexit = 0;
char radix = 'n', rsash[1024], odcmd[1024], rsacmd[1024], odrawdata[8192];
const char* sslprivkeycmd = "openssl rsa -in rsa.pk8 -pubout | tee rsa.spki ";
const char* percent = "%";


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
    printf("\t<form method=\"post\" action=\"/cgi/rsakey.cgi\" id=\"form1\">\n");
    printf("\t\t<div class=\"odDiv\">\n");

    if (hexwidth < 1024)
        printf("\t\t\t<span class=\"leftSpan\"><span class=\"textSpan\">key bytes: </span><select name=\"DropDown_KeyBytesWidth\" onchange=\"FormSubmit()\" id=\"DropDown_KeyBytesWidth\" title=\"Hexadecimal width\" class=\"DropDownList\"><option value=\"512\" selected=\"selected\">512</option><option value=\"1024\">1024</option><option value=\"2048\">2048</option><option value=\"4096\">4096</option><option value=\"8192\">8192</option></select></span>\n");
    if (hexwidth == 1024)
        printf("\t\t\t<span class=\"leftSpan\"><span class=\"textSpan\">key bytes: </span><select name=\"DropDown_KeyBytesWidth\" onchange=\"FormSubmit()\" id=\"DropDown_KeyBytesWidth\" title=\"Hexadecimal width\" class=\"DropDownList\"><option value=\"512\">512</option><option value=\"1024\" selected=\"selected\">1024</option><option value=\"2048\">2048</option><option value=\"4096\">4096</option><option value=\"8192\">8192</option></select></span>\n");
    else if (hexwidth == 2048)
        printf("\t\t\t<span class=\"leftSpan\"><span class=\"textSpan\">key bytes: </span><select name=\"DropDown_KeyBytesWidth\" onchange=\"FormSubmit()\" id=\"DropDown_KeyBytesWidth\" title=\"Hexadecimal width\" class=\"DropDownList\"><option value=\"512\">512</option><option value=\"1024\">1024</option><option value=\"2048\" selected=\"selected\">2048</option><option value=\"4096\">4096</option><option value=\"8192\">8192</option></select></span>\n");

    else if (hexwidth == 4096)
        printf("\t\t\t<span class=\"leftSpan\"><span class=\"textSpan\">key bytes: </span><select name=\"DropDown_KeyBytesWidth\" onchange=\"FormSubmit()\" id=\"DropDown_KeyBytesWidth\" title=\"Hexadecimal width\" class=\"DropDownList\"><option value=\"512\">512</option><option value=\"1024\">1024</option><option value=\"2048\">2048</option><option value=\"4096\" selected=\"selected\">4096</option><option value=\"8192\">8192</option></select></span>\n");
    else if (hexwidth == 8192)
        printf("\t\t\t<span class=\"leftSpan\"><span class=\"textSpan\">key bytes: </span><select name=\"DropDown_KeyBytesWidth\" onchange=\"FormSubmit()\" id=\"DropDown_KeyBytesWidth\" title=\"Hexadecimal width\" class=\"DropDownList\"><option value=\"512\">512</option><option value=\"1024\">1024</option><option value=\"2048\">2048</option><option value=\"4096\">4096</option><option value=\"8192\" selected=\"selected\">8192</option></select></span>\n");
    
    printf("\t\t\t<span style=\"width:22%s; font-size: larger; text-align: left;\"><input type=\"submit\" name=\"Button_RsaKeyGen\" value=\"rsa key gen\" id=\"Button_RsaKeyGen\" title=\"Click to generate rsa  key pair.\" /></span>\n", percent);

    printf("\t\t\t</div>\n");

    printf("\t\t\t<div class=\"odDiv\">\n");

    printf("\t\t\t<span class=\"smallSpan\" style=\"width:74%s; text-align: right;\"><span class=\"textSpan\">rsa key gen: </span><input type=\"text\" value=\"%s\" maxlength=\"128\" readonly=\"ReadOnly\" name=\"TextBox_RsaCmd\" id=\"TextBox_RsaCmd\" title=\"rsa shell command\" class=\"ButtonTextBox\" style=\"height:24pt; width:68%c;\" /></span>\n", percent, odcmd, percent[0]);

    printf("\t\t\t</div>\n");
    printf("\t\t\t<hr />\n\t\t\t");
    printf("<pre id=\"odraw\"  name=\"odraw\" style=\" width: 520px; margin: 3 3 3 3; padding-left: 2px; background-color: #dfdfdf;\">");
    // font-family: monospace, ui-monospace, system-ui, sans-serif;

    while (fgets(data, DATA_SIZE, pf))
    {
        printf("%s", data);
        fexit = 0;
    }
    (void)fclose(pf);

    printf("</pre>\n");
    printf("\t\t\t<hr />\n");

    printf("\t\t\t<div align=\"left\" class=\"footerDiv\"><span style=\"width:12%s; text-align: left;\" align=\"left\" valign=\"middle\"><a href=\"/cgi/fortune.cgi\">fortune</a></span>", percent);

    printf("<span style=\"width:12%c; text-align: center;\" align=\"center\" valign=\"middle\"><a href=\"/net/\">.net c# demo apache2 mod_mono</a></span>", '%');

    printf("<span style=\"width:12%c; text-align: center;\" align=\"center\" valign=\"middle\"><a href=\"/cgi/od.cgi\">octal dump</a></span>", percent[0]);

    printf("<span style=\"width:64%s; text-align: right;\" align=\"right\" valign=\"middle\"><a href=\"mailto:he@area23.at\">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href=\"https://blog.area23.at\">blog.</a>]<a href=\"https://area23.at\">area23.at</a></span></div>\n", percent);
    printf("\t</form>\n");
    printf("\t</body>\n\n</html>\n");

    fflush(stdout);

    return fexit;
}



int saveDump(char* rawdump) {
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

    printf("<!-- %s -->\n", strchr(rawdump, ((int)13)));
    printf("<!-- strlen rawdump = %d -->\n", (int)strlen(rawdump));

    printf("<-- \n %s \n", rawdump);
    printf("\t\t-->\n");

    fprintf(fp, "%s\n", rawdump);
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
        sprintf(rsacmd, "/usr/local/bin/rsacmd.sh %d ", hexwidth);

        sprintf(odcmd,
            "openssl genpkey -algorithm rsa -pkeyopt rsa_keygen_bits:%d > rsa.pk8 ; %s ; cat rsa.pk8",
            hexwidth, sslprivkeycmd);

        return exeCmd(rsacmd);
    }

    // saveDump(postdata);


    while (fgets(postdata, len + 1, stdin))         /* work with postdata */
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
                if ((scmp = strcmp(token, "DropDown_KeyBytesWidth")) == 0)
                    hexwidth = strtol(subtoken, NULL, 10);
                else if ((scmp = strcmp(token, "DropDown_WordWidth")) == 0)
                    wordwidth = strtol(subtoken, NULL, 10);
                else if ((scmp = strcmp(token, "TextBox_RsaCmd")) == 0)
                    sprintf(rsash, "%s", subtoken);
                else if ((scmp = strcmp(token, "odraw")) == 0)
                    sprintf(odrawdata, "%s", subtoken);
            }
        }
    }

    sprintf(odcmd,
          "openssl genpkey -algorithm rsa -pkeyopt rsa_keygen_bits:%d > rsa.pk8 ; %s ; cat rsa.pk8",
          hexwidth, sslprivkeycmd);

    sprintf(rsacmd, "/usr/local/bin/rsakey.sh %d", hexwidth);

    pexit = exeCmd(rsacmd);

    // saveDump(odrawdata);
    // saveDump(postdata);

    (void)free(postdata);

    return pexit;
}
