import { Box, Button, Paper, TextField, Typography } from '@mui/material'
import { FormEvent } from 'react';
import { useActivities } from '../../../lib/hooks/useActivities';
import { useNavigate, useParams } from 'react-router';

export default function ActivityForm() {
    const {id} = useParams();
    const {updateActivity, createActivity, activity, isLoadingActivity} = useActivities(id);
    const navigate = useNavigate();
    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        const formData = new FormData(event.currentTarget);

        const data: { [key: string]: FormDataEntryValue } = {}
        formData.forEach((value, key) => {
            data[key] = value;
        });
        console.log(data);

        if (activity) {
            data.id = activity.id
            await updateActivity.mutateAsync(data as unknown as Activity)
            navigate(`/activities/${activity.id}`)
        } else {
            createActivity.mutate(data as unknown as Activity, {
                onSuccess: (id) => {
                    navigate(`/activities/${id}`)
                }
            });
        }
    }

    if(isLoadingActivity) return <h1>Loading...</h1>
    return (
        <Paper>
            <Typography variant='h5' gutterBottom color='primary'>
                {activity ? 'Edit Activity' : 'Create Activity'}
            </Typography>
            <Box onSubmit={handleSubmit} component='form' display='flex' flexDirection='column' sx={{ mt: 3 }}>
                <TextField name='title' label='Title' variant='outlined' sx={{ mb: 2 }} defaultValue={activity?.title} />
                <TextField name='description' label='Description' variant='outlined' defaultValue={activity?.description} multiline rows={3} sx={{ mb: 2 }} />
                <TextField name='category' label='Category' variant='outlined' defaultValue={activity?.category} sx={{ mb: 2 }} />
                <TextField name='date' label='Date' variant='outlined' type='date' sx={{ mb: 2 }}
                            defaultValue={activity?.date
                                ? new Date(activity.date).toISOString().split('T')[0]
                                : new Date().toISOString().split('T')[0]
                            } />
                <TextField name='city' label='City' variant='outlined' defaultValue={activity?.city} sx={{ mb: 2 }} />
                <TextField name='venue' label='Venue' variant='outlined' defaultValue={activity?.venue} sx={{ mb: 2 }} />
                <Box display='flex' justifyContent='end' gap={3}>
                    <Button onClick={() => navigate(`/activities/${id}`)} color='inherit'>Cancel</Button>
                    <Button type='submit' variant='contained' color='success' disabled={updateActivity.isPending || createActivity.isPending}>Submit</Button>
                </Box>
            </Box>
        </Paper>

    )
}
