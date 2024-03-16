import requests
import json
import hmac
import hashlib
from urllib.parse import quote_plus
import time
import configparser

class PAPAGO:
    config_path = "TranslationTool.ini"
    SOURCE = "en"
    TARGET = "ko"
    HONORIFIC = "false"

    @staticmethod
    def load():
        config = configparser.ConfigParser()
        config.read(PAPAGO.config_path)
        PAPAGO.SOURCE = config.get("PAPAGO", "SOURCE", fallback="en")
        PAPAGO.TARGET = config.get("PAPAGO", "TARGET", fallback="ko")
        PAPAGO.HONORIFIC = config.getboolean("PAPAGO", "HONORIFIC", fallback=False)

    @staticmethod
    def save():
        config = configparser.ConfigParser()
        config.read(PAPAGO.config_path)
        if 'PAPAGO' not in config.sections():
            config.add_section('PAPAGO')
        config.set('PAPAGO', 'SOURCE', PAPAGO.SOURCE)
        config.set('PAPAGO', 'TARGET', PAPAGO.TARGET)
        config.set('PAPAGO', 'HONORIFIC', str(PAPAGO.HONORIFIC).lower())
        with open(PAPAGO.config_path, 'w') as configfile:
            config.write(configfile)

    @staticmethod
    def translate(text):
        try:
            uri = "https://papago.naver.com/apis/n2mt/translate"
            query = f"honorific={str(PAPAGO.HONORIFIC).lower()}&source={PAPAGO.SOURCE}&target={PAPAGO.TARGET}&text={quote_plus(text)}"
            
            # 키 입력
            secret_key = "your_secret_key_here".encode('utf-8')
            message = f"{uri}{time.time()}".encode('utf-8')
            
            signature = hmac.new(secret_key, message, hashlib.md5).hexdigest()
            headers = {
                "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
                "X-Naver-Client-Id": "your_client_id",
                "X-Naver-Client-Secret": "your_client_secret",
                # 인증 ID 입
            }
            
            response = requests.post(uri, data=query, headers=headers)
            response_json = response.json()
            return response_json.get('message', {}).get('result', {}).get('translatedText', '')
        except Exception as e:
            print(f"Error: {e}")
            return ""

if __name__ == "__main__":
    PAPAGO.load()
    translated_text = PAPAGO.translate("Hello World")
    print(f"Translated Text: {translated_text}")

    PAPAGO.SOURCE = "ko"
    PAPAGO.TARGET = "en"
    PAPAGO.save()
