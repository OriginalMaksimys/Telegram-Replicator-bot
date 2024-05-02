print("Replycator API 0.01")

import os 

def delete_files():
    for filename in os.listdir("files/"):
        os.remove("files/" + filename)

def send(mode, file_type=None):

    if mode == 'social':

        if file_type == 'all':
            check_send = os.system('sh replycator.sh --social=twitter --text=true --files=true')
        else:
            check_send = os.system('sh replycator.sh --social=twitter --files=true')

        if check_send == 0:
            delete_files()
            return True
        elif check_send == 1:
            return False

    elif mode == 'youtube':
        os.system('sh replycator.sh --social=youtube')

        if check_send == 0:
            delete_files()
            return True
        elif check_send == 1:
            return False