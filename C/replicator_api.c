#include <stdio.h>
#include <stdlib.h>
#include <dirent.h>
#include <unistd.h>
#include <sys/stat.h>
#include <sys/types.h>

void delete_files() {
    DIR *dir;
    struct dirent *entry;
    char path[1024];

    if ((dir = opendir("files/")) != NULL) {
        while ((entry = readdir(dir)) != NULL) {
            if (entry->d_type == DT_REG) {
                sprintf(path, "files/%s", entry->d_name);
                remove(path);
            }
        }
        closedir(dir);
    }
}

int send(const char *mode, const char *file_type) {
    int check_send;

    if (strcmp(mode, "social") == 0) {
        if (file_type != NULL && strcmp(file_type, "all") == 0) {
            check_send = system("sh replycator.sh --social=twitter --text=true --files=true");
        } else {
            check_send = system("sh replycator.sh --social=twitter --files=true");
        }

        if (check_send == 0) {
            delete_files();
            return 1;
        } else if (check_send == 1) {
            return 0;
        }
    } else if (strcmp(mode, "youtube") == 0) {
        check_send = system("sh replycator.sh --social=youtube");

        if (check_send == 0) {
            delete_files();
            return 1;
        } else if (check_send == 1) {
            return 0;
        }
    }

    return 0;
}