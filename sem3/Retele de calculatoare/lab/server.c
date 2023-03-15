// server

#include <sys/types.h>
#include <sys/socket.h>
#include <stdio.h>
#include <netinet/in.h>
#include <netinet/ip.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <signal.h>

int s;

void deservire(int c){
	
	// deservire	

	close(c);
	exit(0);
}

void stop(int sgn){
	printf("\nServer oprit.\n");
	close(s);
	exit(0);
}

int main(){

	struct sockaddr_in server, client;
	int c, l;

	signal(SIGINT, stop);

	s = socket(AF_INET, SOCK_STREAM, 0);
	if(s < 0){
		printf("Eroare la crearea sockt-ului server.\n");
		return 1;
	}

	memset(&server, 0, sizeof(server));
	server.sin_port = htons(1234);
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;

	if(bind(s, (struct sockaddr *) &server, sizeof(server)) < 0){
		printf("Eroare la bind.\n");
		return 1;
	}

	listen(s, 5);
	
	l = sizeof(client);
	memset(&client, 0, sizeof(client));

	while(1){
		c = accept(s, (struct sockaddr *) &client, (int *) &l);
		printf("S-a conectat un client.\n");

		if(fork() == 0){
			deservire(c);
		}
	}

	return 0;
}
