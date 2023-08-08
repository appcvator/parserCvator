import requests
from bs4 import BeautifulSoup
import json
def zarplata(link_):

    if '?' in str(link_):
        id_ = link_.replace('?', ' ').replace('id', ' ').split(' ')[1]

    else:
        id_ = str(link_ + '?').replace('?', ' ').replace('id', ' ').split(' ')[1]

    headers = {
        'Host': 'api.zp.ru',
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/115.0',
    }

    params = {
        'rubric_filter_mode': 'new',
        'increment_views_counter': 'true',
    }
    response = requests.get(f'https://api.zp.ru/v1/vacancies/{id_}', params=params, headers=headers)


    try:
        title = response.json()['vacancies'][0]['header']      # название вакансии
        # print('title', title)
    except:
        title = ''
    try:
        canonical_url = response.json()['vacancies'][0]['canonical_url']       # урл вакансии
        # print('canonical_url', canonical_url)
    except:
        canonical_url = ''
    try:
        company = response.json()['vacancies'][0]['company']['title']       # имя компании
        # print('company', company)
    except:
        company = ''
    try:
        company_url = 'https://chelyabinsk.zarplata.ru'+str(response.json()['vacancies'][0]['company']['url'])  # ссылка на компанию
        # print('company_url', company_url)
    except:
        company_url = ''
    try:
        country = ''                    # страна
        # print('country', country)
    except:
        country = ''
    try:
        contact = response.json()['vacancies'][0]['contact']['city']['title']   # город компании
        # print('contact', contact)
    except:
        contact = ''
    try:
        job_type = response.json()['vacancies'][0]['schedule']['title']     # full time
        # print('job_type', job_type)
    except:
        job_type = ''
    try:
        salary_min = response.json()['vacancies'][0]['salary_min']      # мин зп
        # print('salary_min', salary_min)
    except:
        salary_min = ''
    try:
        salary_max = response.json()['vacancies'][0]['salary_max']      # макс зп
        # print('salary_max', salary_max)
    except:
        salary_max = ''
    try:
        currency = response.json()['vacancies'][0]['currency']['alias']     # валюта
        # print('currency', currency)
    except:
        currency = ''
    try:
        post_date = response.json()['vacancies'][0]['add_date'].split('T')[0]   # дата публикации вакансии
        # print('post_date', post_date)
    except:
        post_date = ''
    try:
        introduction = ''
        # print('introduction', introduction)
    except:
        introduction = ''
    try:
        experience_required = response.json()['vacancies'][0]['experience_length']['title']  # опыт
        # print('experience_required', experience_required)
    except:
        experience_required = ''
    try:
        skills = ''
        # print('skills', skills)
    except:
        skills = ''
    try:
        description = BeautifulSoup(response.json()['vacancies'][0]['description'], 'lxml').text    # описание
        # print('description', description)
    except:
        description = ''
    dict_json = {
          "title": title,
          "url": canonical_url,
          "company": company,
          "company_url": company_url,
          "country": "",
          "city": contact,
          "job_type": job_type,
          "salary_min": salary_min,
          "salary_max": salary_max,
          "currency": currency,
          "post_date": post_date,
          "introduction": "",
          "experience_required": experience_required,
          "skills": '',
          "description": description,
        }
    print(dict_json)
    with open('dict.json', 'w', encoding='utf-8') as f:
        # сохраняем словарь в файл
        json.dump(dict_json, f)

if __name__ == '__main__':
    input_ = input('Введите ссылку на вакансию: ')
    zarplata(input_)



