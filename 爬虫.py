from bs4 import BeautifulSoup
from multiprocessing import Pool
import requests
import pymongo

client = pymongo.MongoClient('localhost', 27017)
typing = client['typing']
section_data = typing['section_data']


def Get_section_data(data):
    url = data["url"]
    list_view = 'https://www.typing.com/student/lessons/{}'.format(url)
    wb_data = requests.get(list_view)
    soup = BeautifulSoup(wb_data.text, 'lxml')
    script_str = str(soup.select('script')[2]).split("\n")[12].replace("    ", "")[20:-3]
    section_list = script_str.split("},{")
    for id_3, one_section in enumerate(section_list):
        data_str = one_section.split('","')
        soup = BeautifulSoup(data_str[4].split(":")[1][1:-1], 'lxml')
        intro = soup.get_text()
        if (intro == 'ull,"sort_order'):
            intro = ""
        data = {
            "url": url,
            "id_1": data["id_1"],
            "id_2": data["id_2"],
            "id_3": id_3,
            "lesson": data["lesson"],
            "chapter": data["chapter"],
            "section": data_str[2].split(":")[1][1:],
            "content": data_str[3].split(":")[1][1:],
            "intro": intro
        }
        section_data.insert_one(data)
        print(data["id_1"], data["id_2"], data["id_3"])


if __name__ == '__main__':
    list_view = 'https://www.typing.com/student'
    wb_data = requests.get(list_view)
    soup = BeautifulSoup(wb_data.text, 'lxml')
    script_str = str(soup.select('script')[2]).split("\n")[10].replace("    ", "")[20:-3]
    lesson_list = script_str.split("},{")[:-2]
    id_switch = {
        "beginner": 0,
        "intermediate": 1,
        "advanced": 2,
        "practice": 3
    }
    id_list = [0, 0, 0, 0]
    task_list = []
    for one_lesson in lesson_list:
        data_str = one_lesson.split(',"')
        lesson = data_str[1].split(":")[1][1:-1]
        id_1 = id_switch.get(lesson)
        id_2 = id_list[id_1]
        data = {
            "url": data_str[0].split(":")[1],
            "id_1": id_1,
            "id_2": id_2,
            "lesson": lesson,
            "chapter": data_str[3].split(":")[1][1:-1]
        }
        id_list[id_1] = id_list[id_1] + 1
        task_list.append(data)

    pool = Pool()
    pool.map(Get_section_data, task_list)
