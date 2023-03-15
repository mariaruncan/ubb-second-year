#include <sys/types.h>
#include <sys/socket.h>
#include <stdio.h>
#include <netinet/in.h>
#include <netinet/ip.h>
#include <string.h>
#include <arpa/inet.h>

int main(){

        int c;
        struct sockaddr_in server;

        c = socket(AF_INET, SOCK_STREAM, 0);
        if(c < 0){
                printf("Eroare la crearea socket-ului client.\n");
                return 1;
        }

        memset(&server, 0, sizeof(server));
        server.sin_port = htons(1234);
        server.sin_family = AF_INET;
        server.sin_addr.s_addr = inet_addr("127.0.0.1");

        if(connect(c, (struct sockaddr *) &server, sizeof(server)) < 0){
                printf("Eroare la conectarea la server.\n");
                return 1;

        }

        // ceea ce vreau sa fac
        char str[100], ch;
        printf("Introduceti sirul: ");
        scanf("%s", str);
        printf("Introduceti caracterul: ");
        scanf("%c", &ch);

        uint16_t l;
        l = strlen(str);
        l = htons(l);
        send(c, &l, sizeof(l), 0);
        send(c, str, l, 0);
        send(c, &ch, sizeof(char), 0);

        recv(c, &l, sizeof(l), 0);
        l = ntohs(l);
        recv(c, str, l,0);
        str[l] = '\0';
        printf("%s", str);

        close(c);

        return 0;
}
