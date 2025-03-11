#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define DATA_SIZE 65536

int pexit = 0;
char fortunecmd[64], fortunelong[64];

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
    while (fgets(data, DATA_SIZE, pf))
    {
        printf("%s", data);
        fexit = 0;
    }
    (void)fclose(pf);
    fflush(stdout);

    return fexit;
}

int main(int argc, char** argv)
{
    char fortuneOp = 's';
    char* postdata = NULL;
    int i = 0, len, scmp;

    // printf("Content-type:text/plain\n\n");
    printf("Content-type: text/html\n\n");
    sprintf(fortunecmd, "/usr/games/fortune -a -%c", fortuneOp);

    printf("<!DOCTYPE html>\n");
    printf("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
    printf("<head>\n");
    printf("\t<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n");
    printf("\t<link rel=\"stylesheet\" href=\"/css/fortune.css\" />\n");
    printf("\t<noscript><meta content=\"16; url=https://area23.at/cgi/fortune.cgi\" http-equiv=\"refresh\" /></noscript>\n");
    printf("\t<title>fortune cgi</title>\n");
    printf("</head>\n");
    printf("<body onload=\"setTimeout(function () { window.location.reload(); }, 16000); return false;\">\n");
    printf("\t<form id=\"form1\" action=\"/cgi/fortune.cgi\">\n");
    printf("\t\t<div class=\"fortuneDiv\" align=\"left\"><p>\n");

    char* len_ = getenv("CONTENT_LENGTH");
    if (len_ != NULL)
    {
        len = strtol(len_, NULL, 10);
        // postdata = malloc(len + 1);
        postdata = calloc((len + 1), sizeof(char));
    }
    if (len != 0 || postdata == NULL)
    {
        pexit = exeCmd(fortunecmd);
    }
    else
    {
        while (fgets(postdata, len + 1, stdin))         /* work with postdata */
        {
            pexit = exeCmd(fortunecmd);
        }
    }

    printf("\t\t</p></div>\n");
    printf("\t\t<hr />\n");
    printf("\t\t<pre id=\"PreFortune\">");

    (void)free(postdata);

    sprintf(fortunelong, "/usr/games/fortune -a -l");
    pexit = exeCmd(fortunelong);

    printf("</pre>\n\t\t<hr />\n");
    printf("\t\t<div class=\"fortuneFooter\" align=\"left\">\n");
    printf("\t\t\t<span class=\"fortuneFooterLeft\" align=\"left\" valign=\"middle\"><a href=\"/cgi/fortune.cgi\">fortune</a></span>\n");
    printf("\t\t\t<span class=\"fortuneFooterLeftCenter\" align=\"center\" valign=\"middle\"><a href=\"/froga/\">froga</a></span>\n");
    printf("\t\t\t<span class=\"fortuneFooterCenter\" align=\"center\" valign=\"middle\"><a href=\"/cgi/od.cgi\">octal dump</a></span>\t\t\t\n");
    printf("\t\t\t<span class=\"fortuneFooterRightCenter\" align=\"center\" valign=\"middle\"><a href=\"/mono/SchnapsNet/\">schnapsen 66</a></span>\n");
    printf("\t\t\t<span class=\"fortuneFooterRight\" align=\"right\" valign=\"middle\"><a href=\"mailto:zen@area23.at\">Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href=\"http://blog.area23.at\">blog.</a>]<a href=\"https://area23.at\">area23.at</a></span>\n");
    printf("\t\t</div>\n");
    printf("\t</form>\n");
    printf("</body>\n");
    printf("</html>\n");
    fflush(stdout);

    return pexit;
}
