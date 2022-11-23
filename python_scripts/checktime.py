import threading
import sys
import uuid
from time import sleep
from datetime import datetime
import pika
import json
import os

basepath = '\\\\srv004bgc.dir.slb.com\\MaxwellCI\\master\\Test\\IBC'

def creat_time(folder):
    testStageBasePath = os.path.join(basepath, folder)
    return os.stat(testStageBasePath).st_ctime

def processOneStage(testStage, verboseLog=False):
    testStageBasePath = os.path.join(basepath, testStage)
    for entry in os.listdir(testStageBasePath):
        happyPathFolder = os.path.join(testStageBasePath, entry)
        if entry.endswith("Happy_Path") and os.path.isdir(happyPathFolder):
            serviceLogFolder = os.path.join(happyPathFolder, "ServicesLogs")
            if not os.path.isdir(serviceLogFolder):
                continue
            for file in os.listdir(serviceLogFolder):
                if file.startswith("State.Service"):
                    with open(os.path.join(serviceLogFolder, file), 'r') as fp:                            
                        old_time = -1
                        old_diff = 0
                        new_diff = 0
                        max_diff = 0
                        started = False
                        while True:
                            line = fp.readline()
                            if verboseLog:                          
                                print(line)
                            if line is None or len(line) == 0:
                                break
                            if not started:
                                if "ChannelService initialize finished." in line:
                                    started = True
                                    if verboseLog:                          
                                        print("start elapsed time calculating")
                                continue
                            #print(len(line))
                            strtime = line[:14]
                            digits = strtime.split(":")
                            # print(digits)
                            # print(digits[0].isnumeric())
                            if len(digits) != 3 and not digits[0].isnumeric():
                                #print("skip")
                                continue
                            new_time = float(digits[0])* 3600000 + float(digits[1])*60000 + float(digits[2])*1000                            
                            if old_time != -1:
                                new_diff = new_time - old_time
                            if new_diff > max_diff:
                                max_diff = new_diff
                            old_time = new_time
                            if verboseLog:
                                print(new_diff)
                        print("process " + testStage + ", max_diff = " + str(max_diff))
                    break
            #print("process finished")
            break


def main():

    if(len(sys.argv) <2):
        print ("wrong arguments, check src to find supported argument.")
        exit(1)

    count = int(sys.argv[1])

    if len(sys.argv) ==3 and count == 1:
        testStage = sys.argv[2]
        processOneStage(testStage, True)
        return

    directories = os.listdir(basepath)
    directories.sort(key=creat_time, reverse=True)
    for testStage in directories:
        processOneStage(testStage)
        count -= 1
        if count <= 0:
            break

if __name__ == '__main__':
    main()