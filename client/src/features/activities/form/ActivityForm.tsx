import { Box, Button, Paper, TextField, Typography } from '@mui/material'
import { FormEvent } from 'react';

type Props = {
    activity?: Activity;
    closeForm: () => void;
    submitForm: (activity: Activity) => void
}
export default function ActivityForm({ activity, closeForm, submitForm }: Props) {
    const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        const formData = new FormData(event.currentTarget);

        const data: { [key: string]: FormDataEntryValue } = {}
        formData.forEach((value, key) => {
            data[key] = value;
        });
        console.log(data);

        if (activity) data.id = activity.id

        submitForm(data as unknown as Activity)
    }
    return (
        <Paper>
            <Typography variant='h5' gutterBottom color='primary'>
                Create activity
            </Typography>
            <Box onSubmit={handleSubmit} component='form' display='flex' flexDirection='column' sx={{ mt: 3 }}>
                <TextField name='title' label='Title' variant='outlined' sx={{ mb: 2 }} defaultValue={activity?.title} />
                <TextField name='description' label='Description' variant='outlined' defaultValue={activity?.description} multiline rows={3} sx={{ mb: 2 }} />
                <TextField name='category' label='Category' variant='outlined' defaultValue={activity?.category} sx={{ mb: 2 }} />
                <TextField name='date' label='Date' variant='outlined' defaultValue={activity?.date} type='date' sx={{ mb: 2 }} />
                <TextField name='city' label='City' variant='outlined' defaultValue={activity?.city} sx={{ mb: 2 }} />
                <TextField name='venue' label='Venue' variant='outlined' defaultValue={activity?.venue} sx={{ mb: 2 }} />
                <Box display='flex' justifyContent='end' gap={3}>
                    <Button onClick={closeForm} color='inherit'>Cancel</Button>
                    <Button type='submit' variant='contained' color='success'>Submit</Button>
                </Box>
            </Box>
        </Paper>

    )
}
