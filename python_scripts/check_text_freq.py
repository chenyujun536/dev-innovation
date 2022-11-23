import sys
import os
import pprint

def main():

    if(len(sys.argv) < 2):
        print("wrong arguments, check src to find supported argument.")
        exit(1)

    file = os.path.join(os.getcwd(), sys.argv[1])

    words={}

    with open(file, 'r') as fp:
        while True:
            line = fp.readline()
            if line is None or len(line) == 0:
                break
            word_list = line.split(" - ")
            if len(word_list) < 2:
                continue
            logs = word_list[1].split(",")
            log = logs[0].replace(":","@")[0:40]

            if "Updating BoundPass from" in log:
                log = log[log.index("]"):]  # remove channel name in this log
            

            if log not in words:
                words[log] = 1
            else:
                words[log] = words[log] + 1
    
    sorted_words = {k: v for k, v in sorted(words.items(), key=lambda item: item[1])}

    pprint.pprint(sorted_words)


if __name__ == '__main__':
    main()
