
import requests
from bs4 import BeautifulSoup
import json

def naukri(id_):
    headers = {
        'Host': 'www.naukri.com',
        'Accept-Language': 'ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3',
        'Appid': '121',
        'Systemid': 'Naukri',
    }
    if '?' in str(id_):
        link_transform = id_.replace('-', ' ').replace('?', ' ').split(' ')[-2]
        # print('1\n', link_transform)
    else:
        link_transform = str(id_ + '?').replace('-', ' ').replace('?', ' ').split(' ')[-2]
        # print('2\n', link_transform)

    response = requests.get(
        f'https://www.naukri.com/jobapi/v4/job/{link_transform}', headers=headers)
    res = response.json()
    try:
        title = res['jobDetails']['title']
        # print('title', title)
    except:
        title = ''
    try:
        url_vac = res['jobDetails']['staticUrl']
        # print('url_vac', url_vac)
    except:
        url_vac = ''
    try:
        company_name = res['jobDetails']['companyDetail']['name']
        # print('company_name', company_name)
    except:
        company_name = ''
    try:
        company_url = res['jobDetails']['companyDetail']['websiteUrl']
        # print('company_url', company_url)
    except:
        company_url = ''
    try:
        country = ''
        # print('country', country)
    except:
        country = ''
    try:
        city = ''
        for i in res['jobDetails']['locations']:
            city_ = str(i['label']) + '\n'
            city += city_
        # print('city', city)
    except:
        city = ''
    try:
        job_type = res['jobDetails']['employmentType']
        # print('job_type', job_type)
    except:
        job_type = ''
    try:
        salary_min = res['jobDetails']['salaryDetail']['minimumSalary']
        # print('salary_min', salary_min)
    except:
        salary_min = ''
    try:
        salary_max = res['jobDetails']['salaryDetail']['maximumSalary']
        # print('salary_max', salary_max)
    except:
        salary_max = ''
    try:
        currency = res['jobDetails']['salaryDetail']['currency']
        # print('currency', currency)
    except:
        currency = ''
    try:
        post_date = res['jobDetails']['createdDate']
        # print('post_date', post_date)
    except:
        post_date = ''
    try:
        introduction = ''
        # print('introduction', introduction)
    except:
        introduction = ''
    try:
        experience_required1 = str(res['jobDetails']['minimumExperience'])
    except:
        experience_required1 = ''
    try:
        experience_required2 = '-' + str(res['jobDetails']['maximumExperience'])
    except:
        experience_required2 = ''
    experience_required = experience_required1 + experience_required2
    # print('experience_required', experience_required)
    try:
        skills = ''
        for i in res['jobDetails']['keySkills']['other']:
            skills1 = str(i['label'])+'\n'
            skills += skills1
        # print('skills', skills)
    except:
        skills = ''
    try:
        description = BeautifulSoup(res['jobDetails']['description'], 'lxml').text
        # print('description', description)
    except:
        description = ''

    dict_json = {
        "title": title,
        "url": url_vac,
        "company": company_name,
        "company_url": company_url,
        "country": country,
        "city": city,
        "job_type": job_type,
        "salary_min": salary_min,
        "salary_max": salary_max,
        "currency": currency,
        "post_date": post_date,
        "introduction": introduction,
        "experience_required": experience_required,
        "skills": skills,
        "description": description,
    }
    print(dict_json)
    with open('dict.json', 'w', encoding='utf-8') as f:
        # сохраняем словарь в файл
        json.dump(dict_json, f)

if __name__ == '__main__':
    link_ = input('Link: ')
    naukri(link_)

