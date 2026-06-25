# ER Diagram (govaeb)

```mermaid
erDiagram
    _tmp_tbl_customer {
        int c_id PK
        varchar address
        varchar con_type
        varchar email
        double l1
        double l2
        double maxunits
        varchar metno
        varchar c_name
        varchar password
        varchar phone
        double units
        double 11
        double 12
        double max_units
        varchar met_no
        varchar cname
    }

    customer {
        int c_id PK
        varchar address
        varchar c_name
        varchar con_type
        varchar email
        double l1
        double l2
        double max_units
        varchar met_no
        varchar password
        varchar phone
        double units
    }

    bills {
        int id PK
        double amount
        datetime date
        varchar met_no
        varchar paid_date
        varchar status
        int c_id FK
        varchar metno
        datetime time_stamp
        int customer_id FK
    }

    tblCustomer {
        int cId PK
        varchar cName
        varchar metno
        varchar Email
        varchar Address
        varchar phone
        varchar con_type
        varchar password
        double units
        double maxunits
        double l1
        double l2
    }

    tblBill {
        int Id PK
        varchar metno
        timestamp date
        varchar status
        double amount
        int cId FK
        varchar PaidDate
    }

    authfactor {
        smallint reqid PK
        varchar status
        smallint sessiontimeout
    }

    controlrequest {
        smallint reqid PK
        varchar securekey
        binary encryotp
        varchar otp
        varchar sessionkey
        timestamp verifiedstamp
        varchar status
    }

    request {
        smallint reqid PK
        tinytext username
        tinytext macid
        tinytext ipaddress
        tinytext macname
        tinytext datetime
        varchar status
        varchar certificate
        varchar privatekey
        varchar publickey
    }

    datalogger {
        int slno PK
        varchar macid
        varchar stamp
        blob edata
    }

    patch {
        smallint patchid PK
        varchar stamp
        blob patchcmd
        varchar macid
    }

    readings {
        smallint id PK
        double data
    }

    blockchain {
        smallint slno
        smallint cid
        varchar units
        varchar hash
    }

    ledger1 {
        smallint slno
        smallint cid
        varchar units
    }

    iotdata {
        float readings
    }

    tblRates {
        varchar type
        float cost
    }

    tbl_bill_seq {
        bigint next_val
    }

    customer ||--o{ bills : "bills.c_id -> customer.c_id"
    _tmp_tbl_customer ||--o{ bills : "bills.customer_id -> _tmp_tbl_customer.c_id"
    tblCustomer ||--o{ tblBill : "tblBill.cId -> tblCustomer.cId"
```

## Notes
- Diagram generated from live `information_schema` metadata.
- `bills` has two foreign keys: one to `customer` and one to `_tmp_tbl_customer`.
- Several tables (`blockchain`, `ledger1`, `iotdata`, `tblRates`, `tbl_bill_seq`) have no PK/FK constraints defined in metadata.
